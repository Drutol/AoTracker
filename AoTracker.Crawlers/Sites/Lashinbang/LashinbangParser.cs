﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Utils;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AoTracker.Crawlers.Sites.Lashinbang
{
    public class LashinbangParser : TypedParser<LashinbangItem, LashinbangSourceParameters>
    {
        private ILogger<LashinbangParser> _logger;

        public LashinbangParser(ILogger<LashinbangParser> logger)
        {
            _logger = logger;
        }

        protected override Task<ICrawlerResultList<LashinbangItem>> Parse(string data,
            LashinbangSourceParameters parameters)
        {
            var root = JsonConvert.DeserializeObject<RootObject>(data.Substring(9).Trim(';',')'));
            var parsedItems = new List<LashinbangItem>();
            var output = new CrawlerResultBase<LashinbangItem>
            {
                Results = parsedItems,
            };

            try
            {
                foreach (var resultItem in root.Kotohaco.Result.Items)
                {
                    if(IsItemExcluded(resultItem.Title, parameters))
                        continue;

                    var item = new LashinbangItem
                    {
                        Id = resultItem.Itemid,
                        InternalId = $"lashin_{resultItem.Itemid}",
                        Name = resultItem.Title,
                        Price = resultItem.Price,
                        ImageUrl = resultItem.Image
                    };

                    parsedItems.Add(item);
                }

                output.Success = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to parse list of items. ({parameters.SearchQuery})");
            }

            return Task.FromResult((ICrawlerResultList<LashinbangItem>)output);
        }

        public override Task<ICrawlerResultSingle<LashinbangItem>> ParseDetail(string data, string id)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            var output = new CrawlerResultBase<LashinbangItem>();
            try
            {
                var item = new LashinbangItem();

                item.Id = id;
                item.InternalId = $"lashin_{item.Id}";
                item.ImageUrl = doc.FirstOfDescendantsWithId("img", "zoom_03").Attributes["data-zoom-image"].Value;
                item.Name = WebUtility.HtmlDecode(doc.FirstOfDescendantsWithClass("div", "item_name").Descendants("h1")
                    .First().InnerText.Trim());
                item.Price = float.Parse(doc.FirstOfDescendantsWithClass("div", "price red").InnerText.Split('円').First()
                    .Replace(",", "").Trim());

                output.Success = true;
                output.Result = item;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to parse item detail ({id}).");
            }

            return Task.FromResult((ICrawlerResultSingle<LashinbangItem>)output);
        }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class Item
        {
            public string Itemid { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }
            public string Desc { get; set; }
            public string Image { get; set; }
            public string Path { get; set; }
            public int Price { get; set; }
        }
        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class Result
        {
            public List<Item> Items { get; set; }
        }
        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class Kotohaco
        {
            public Result Result { get; set; }
        }
        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]

        public class RootObject
        {
            public Kotohaco Kotohaco { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Utils;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace AoTracker.Crawlers.Surugaya
{
    public class SurugayaParser : TypedParser<SurugayaItem, SurugayaSourceParameters>
    {
        private ILogger<SurugayaParser> _logger;

        public SurugayaParser(ILogger<SurugayaParser> logger)
        {
            _logger = logger;
        }

        protected override Task<ICrawlerResultList<SurugayaItem>> Parse(string data, SurugayaSourceParameters parameters)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            var items = doc.DocumentNode.WhereOfDescendantsWithClass("div", "item");
            var parsedItems = new List<SurugayaItem>();
            var output = new CrawlerResultBase<SurugayaItem>
            {
                Results = parsedItems,
            };
            try
            {
                foreach (var itemNode in items)
                {
                    var link = itemNode.FirstOfDescendantsWithClass("p", "title").Descendants("a").First();
                    var priceBlock = itemNode.FirstOfDescendantsWithClass("p", "price_teika");
                    var itemName = WebUtility.HtmlDecode(link.InnerText);

                    if (IsItemExcluded(itemName, parameters))
                        continue;

                    var item = new SurugayaItem();

                    item.Id = link.Attributes["href"].Value.Split('/').Last();
                    item.Name = itemName;
                    if (float.TryParse(priceBlock
                        .Descendants("strong")
                        .First()
                        .InnerText
                        .Replace("￥", "")
                        .Replace(",", ""), out var price))
                    {
                        item.Price = price;
                    }

                    item.ImageUrl = itemNode.Descendants("img").First().Attributes["src"].Value;
                    item.InternalId = $"surugaya_{item.Id}";
                    item.Category =
                        WebUtility.HtmlDecode(itemNode.FirstOfDescendantsWithClass("p", "condition").InnerText);
                    item.Brand = WebUtility.HtmlDecode(itemNode.FirstOfDescendantsWithClass("p", "brand").InnerText);

                    if (parameters.TrimJapaneseQuotationMarks)
                    {
                        item.Name = Regex.Replace(item.Name, "\\「.*\\」", "");
                    }

                    parsedItems.Add(item);
                }

                output.Success = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to parse list of items. ({parameters.SearchQuery})");
            }

            return Task.FromResult((ICrawlerResultList<SurugayaItem>)output);
        }

        public override Task<ICrawlerResultSingle<SurugayaItem>> ParseDetail(string data, string id)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            var output = new CrawlerResultBase<SurugayaItem>();

            try
            {
                var item = new SurugayaItem();

                var titleSection = doc.FirstOfDescendantsWithId("h2", "item_title");
                var categorySpan = titleSection.Descendants("span").First();
                titleSection.RemoveChild(categorySpan);

                item.Id = id;
                item.InternalId = $"surugaya_{item.Id}";
                item.Name = WebUtility.HtmlDecode(titleSection.InnerText.Trim());
                item.Category = WebUtility.HtmlDecode(categorySpan.InnerText.Trim());
                if (data.Contains("申し訳ございません。品切れ中です"))
                    item.Price = CrawlerConstants.InvalidPrice;
                else
                {
                    item.Price = float.Parse(doc.FirstOfDescendantsWithClass("span", "text-red text-bold mgnL10").InnerText
                        .Replace(",", "").Replace("円 (税込)", "").Trim());
                }

                item.Brand = WebUtility.HtmlDecode(
                    doc
                        .WhereOfDescendantsWithClass("td", "t_contents")
                        .FirstOrDefault(node => node.Descendants("a").Any(htmlNode =>
                            htmlNode.Attributes["href"].Value.Contains("category=&search_word=&restrict[]=brand")))?
                        .InnerText?
                        .Trim());
                var image = doc.FirstOfDescendantsWithClass("a", "cloud-zoom");
                item.ImageUrl = image.Attributes["href"].Value;

                output.Success = true;
                output.Result = item;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to parse item detail ({id}).");
            }
            
            return Task.FromResult((ICrawlerResultSingle<SurugayaItem>)output);
        }
    }
}

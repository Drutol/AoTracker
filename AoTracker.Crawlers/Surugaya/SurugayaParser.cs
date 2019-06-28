using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Utils;
using HtmlAgilityPack;

namespace AoTracker.Crawlers.Surugaya
{
    public class SurugayaParser : ICrawlerParser<SurugayaItem>
    {
        public Task<ICrawlerResult<SurugayaItem>> Parse(Stream data)
        {
            var doc = new HtmlDocument();
            doc.Load(data);

            var items = doc.DocumentNode.WhereOfDescendantsWithClass("div", "item");
            var parsedItems = new List<SurugayaItem>();
            var output = new CrawlerResultBase<SurugayaItem>
            {
                Results = parsedItems
            };

            foreach (var itemNode in items)
            {
                var item = new SurugayaItem();

                var link = itemNode.Descendants("a").First();
                var priceBlock = itemNode.FirstOfDescendantsWithClass("p", "price_teika");

                item.Id = link.Attributes["href"].Value.Split('/').Last();
                item.Name = WebUtility.HtmlDecode(link.InnerText);
                item.Price = float.Parse(priceBlock.Descendants("strong").First().InnerText.Replace("￥", "")
                    .Replace(",", ""));
                item.ImageUrl = itemNode.Descendants("img").First().Attributes["src"].Value;

                parsedItems.Add(item);
            }

            return Task.FromResult((ICrawlerResult<SurugayaItem>) output);
        }
    }
}

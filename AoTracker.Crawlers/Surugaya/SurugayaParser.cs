using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Utils;
using HtmlAgilityPack;

namespace AoTracker.Crawlers.Surugaya
{
    public class SurugayaParser : TypedParser<SurugayaItem, SurugayaSourceParameters>
    {
        protected override Task<ICrawlerResult<SurugayaItem>> Parse(string data, SurugayaSourceParameters parameters)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            var items = doc.DocumentNode.WhereOfDescendantsWithClass("div", "item");
            var parsedItems = new List<SurugayaItem>();
            var output = new CrawlerResultBase<SurugayaItem>
            {
                Results = parsedItems,
                Success = true
            };
            try
            {
                foreach (var itemNode in items)
                {
                    var item = new SurugayaItem();

                    var link = itemNode.FirstOfDescendantsWithClass("p", "title").Descendants("a").First();
                    var priceBlock = itemNode.FirstOfDescendantsWithClass("p", "price_teika");

                    item.Id = link.Attributes["href"].Value.Split('/').Last();
                    item.Name = WebUtility.HtmlDecode(link.InnerText);
                    item.Price = float.Parse(priceBlock.Descendants("strong").First().InnerText.Replace("￥", "")
                        .Replace(",", ""));
                    item.ImageUrl = itemNode.Descendants("img").First().Attributes["src"].Value;

                    if (parameters.TrimJapaneseQuotationMarks)
                    {
                        item.Name = Regex.Replace(item.Name, "\\「.*\\」", "");
                    }

                    parsedItems.Add(item);
                }
            }
            catch
            {
                output.Success = false;
            }
            return Task.FromResult((ICrawlerResult<SurugayaItem>)output);
        }
    }
}

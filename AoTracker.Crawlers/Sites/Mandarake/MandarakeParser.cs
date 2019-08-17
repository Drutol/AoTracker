using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Crawlers.Utils;
using HtmlAgilityPack;

namespace AoTracker.Crawlers.Mandarake
{
    public class MandarakeParser : TypedParser<MandarakeItem, MandarakeSourceParameters>
    {
        protected override Task<ICrawlerResult<MandarakeItem>> Parse(string data, MandarakeSourceParameters parameters)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            var items = doc.DocumentNode.FirstOfDescendantsWithClass("div", "entry").WhereOfDescendantsWithClass("div", "block");
            var parsedItems = new List<MandarakeItem>();
            var output = new CrawlerResultBase<MandarakeItem>
            {
                Results = parsedItems,
                Success = true
            };
            try
            {
                foreach (var itemNode in items)
                {
                    var item = new MandarakeItem();

                    var id = itemNode.FirstOfDescendantsWithClass("p", "itemno").InnerText.Trim();
                    var startId = id.IndexOf('(');

                    item.Id = id.Substring(startId + 1).Trim(')');
                    item.Name = WebUtility.HtmlDecode(itemNode.FirstOfDescendantsWithClass("div", "title").InnerText.Trim());
                    item.Price = float.Parse(itemNode.FirstOfDescendantsWithClass("div", "price").InnerText.Replace("円+税", "")
                        .Replace(",", ""));
                    item.ImageUrl = itemNode.Descendants("img").Last().Attributes["src"].Value;
                    item.InternalId = $"mandarake_{item.Id}";
                    item.Shop =
                        WebUtility.HtmlDecode(itemNode.FirstOfDescendantsWithClass("p", "shop").InnerText.Trim());

                    parsedItems.Add(item);
                }
            }
            catch
            {
                output.Success = false;
            }
            return Task.FromResult((ICrawlerResult<MandarakeItem>)output);
        }
    }
}

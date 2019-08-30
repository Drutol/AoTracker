using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Crawlers.Utils;
using HtmlAgilityPack;

namespace AoTracker.Crawlers.Mandarake
{
    public class MandarakeParser : TypedParser<MandarakeItem, MandarakeSourceParameters>
    {
        protected override Task<ICrawlerResultList<MandarakeItem>> Parse(string data, MandarakeSourceParameters parameters)
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
                    var itemName = WebUtility.HtmlDecode(itemNode.FirstOfDescendantsWithClass("div", "title").InnerText.Trim());
                    
                    if (IsItemExcluded(itemName, parameters))
                        continue;

                    var item = new MandarakeItem();

                    var id = itemNode.FirstOfDescendantsWithClass("a", "addbasket").Attributes["data-index"].Value.Trim();

                    item.Id = id;
                    item.Name = itemName;
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
            return Task.FromResult((ICrawlerResultList<MandarakeItem>)output);
        }

        public override Task<ICrawlerResultSingle<MandarakeItem>> ParseDetail(string data, string id)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            var output = new CrawlerResultBase<MandarakeItem>
            {
                Success = true
            };

            var item = new MandarakeItem();

            item.Id = id;
            item.InternalId = $"mandarake_{item.Id}";
            item.Shop = WebUtility.HtmlDecode(doc.FirstOfDescendantsWithClass("div", "shop").InnerText.Trim());
            item.Name = WebUtility.HtmlDecode(doc.FirstOfDescendantsWithClass("div", "subject").InnerText.Trim());

            if (doc.WhereOfDescendantsWithClass("a", "addalert").Any())
                item.Price = CrawlerConstants.InvalidPrice;
            else
                item.Price = float.Parse(doc.FirstOfDescendantsWithClass("p", "__price").InnerText.Split('円').First()
                    .Replace(",", ""));

            item.ImageUrl = doc.FirstOfDescendantsWithClass("img", "xzoom").Attributes["xoriginal"].Value;
            item.ImageUrl = item.ImageUrl.Insert(item.ImageUrl.LastIndexOf('/') + 1, "s_");

            output.Result = item;

            return Task.FromResult((ICrawlerResultSingle<MandarakeItem>)output);
        }
    }
}

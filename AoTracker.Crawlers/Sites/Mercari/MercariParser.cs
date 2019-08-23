using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Utils;
using HtmlAgilityPack;

namespace AoTracker.Crawlers.Sites.Mercari
{
    public class MercariParser : TypedParser<MercariItem, MercariSourceParameters>
    {
        protected override Task<ICrawlerResult<MercariItem>> Parse(string data, MercariSourceParameters parameters)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            var items = doc.DocumentNode.WhereOfDescendantsWithClass("div", "item");
            var parsedItems = new List<MercariItem>();
            var output = new CrawlerResultBase<MercariItem>
            {
                Results = parsedItems,
                Success = true
            };


            try
            {
                var itemNodes = doc
                    .FirstOfDescendantsWithClass("div", "items-box-content clearfix")
                    .WhereOfDescendantsWithClass("section", "items-box");

                foreach (var itemNode in itemNodes)
                {
                    var item = new MercariItem();

                    var image = itemNode.Descendants("img").First();
                    var link = itemNode.Descendants("a").First();

                    var idTemp = link.Attributes["href"].Value;
                    var pos = idTemp.IndexOf('?');
                    idTemp = idTemp.Substring(0, pos).Trim('/');
                    pos = idTemp.LastIndexOf('/');

                    item.Id = idTemp.Substring(pos + 1);
                    item.InternalId = $"mercari_{item.Id}";
                    item.Name = WebUtility.HtmlDecode(image.Attributes["alt"].Value.Trim());
                    item.Price = float.Parse(itemNode.FirstOfDescendantsWithClass("div", "items-box-price font-5")
                        .InnerText.Replace("¥", "").Replace(",", "").Trim());
                    item.ImageUrl = image.Attributes["data-src"].Value;


                    parsedItems.Add(item);
                }
            }
            catch (Exception e)
            {
                output.Success = false;
            }

            return Task.FromResult((ICrawlerResult<MercariItem>) output);
        }
    }
}

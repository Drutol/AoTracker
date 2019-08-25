using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Utils;
using HtmlAgilityPack;

namespace AoTracker.Crawlers.Sites.Mercari
{
    public class MercariParser : TypedParser<MercariItem, MercariSourceParameters>
    {
        protected override Task<ICrawlerResultList<MercariItem>> Parse(string data, MercariSourceParameters parameters)
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

            return Task.FromResult((ICrawlerResultList<MercariItem>) output);
        }

        public override Task<ICrawlerResultSingle<MercariItem>> ParseDetail(string data, string id)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            var output = new CrawlerResultBase<MercariItem>
            {
                Success = true
            };

            var image = doc.FirstOfDescendantsWithClass("img", "owl-lazy");

            var item = new MercariItem();

            item.Id = id;
            item.InternalId = $"mercari_{item.Id}";
            item.Name = WebUtility.HtmlDecode(image.Attributes["alt"].Value.Trim());
            item.ImageUrl = image.Attributes["data-src"].Value;
            if (data.Contains("売り切れました"))
            {
                item.Price = CrawlerConstants.InvalidPrice;
            }
            else
            {
                item.Price = float.Parse(doc.FirstOfDescendantsWithClass("span", "item-price bold")
                    .InnerText.Replace("¥", "").Replace(",", "").Trim());
            }

            output.Result = item;


            return Task.FromResult((ICrawlerResultSingle<MercariItem>)output);
        }
    }
}

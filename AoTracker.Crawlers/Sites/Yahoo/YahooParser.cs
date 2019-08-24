using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Utils;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AoTracker.Crawlers.Sites.Yahoo
{
    class YahooParser : TypedParser<YahooItem, YahooSourceParameters>
    {
        protected override Task<ICrawlerResultList<YahooItem>> Parse(string data, YahooSourceParameters parameters)
        {
            var root = JsonConvert.DeserializeObject<RootObject>(data);
            var parsedItems = new List<YahooItem>();
            var output = new CrawlerResultBase<YahooItem>
            {
                Results = parsedItems,
                Success = true
            };

            foreach (var rootItem in root.Items)
            {
                YahooItem.ItemCondition condition = YahooItem.ItemCondition.Unknown;

                if (rootItem.Condition == "中古")
                    condition = YahooItem.ItemCondition.Used;
                else if (rootItem.Condition == "新品")
                    condition = YahooItem.ItemCondition.New;

                var item = new YahooItem
                {
                    Id = rootItem.Id,
                    InternalId = $"yahoo_{rootItem.Id}",
                    ImageUrl = rootItem.ImageUrl,
                    Name = rootItem.Title,
                    Price = rootItem.Price,
                    BuyoutPrice = rootItem.BuyItNowPrice,
                    EndTime = new DateTimeOffset(DateTime.Parse(rootItem.EndTime), TimeSpan.FromHours(9))
                        .ToUniversalTime().UtcDateTime,
                    IsShippingFree = rootItem.Postage == 0,
                    Condition = condition,
                    Tax = rootItem.Tax,
                    BidsCount = rootItem.Bids,
                };

                parsedItems.Add(item);
            }

            return Task.FromResult((ICrawlerResultList<YahooItem>)output);
        }

        public override Task<ICrawlerResultSingle<YahooItem>> ParseDetail(string data, string id)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            var output = new CrawlerResultBase<YahooItem>
            {
                Success = true
            };

            var image = doc.FirstOfDescendantsWithClass("p", "fjd_img img").Descendants("img").First();

            var item = new YahooItem();
            item.Id = id;
            item.InternalId = $"yahoo_{item.Id}";
            item.ImageUrl = image.Attributes["src"].Value;
            item.Name = WebUtility.HtmlDecode(image.Attributes["alt"].Value);
            if (data.Contains("Sorry: Auction of item URL or Auction ID that you filled in has been closed."))
                item.Price = -1;
            else
            {
                var container = doc.FirstOfDescendantsWithClass("div", "dtl").WhereOfDescendantsWithClass("span", "num")
                    .ToList();
                item.Price = float.Parse(container[0].InnerText.Replace(",", "").Trim());
                if (container.Count == 2)
                {
                    item.BuyoutPrice = float.Parse(container[1].InnerText.Replace(",", "").Trim());
                }
            }

            var detailsGrid = doc.FirstOfDescendantsWithClass("div", "wr");
            var datesRow = detailsGrid.Descendants("tr").First(node => node.InnerText.Contains("End time"));
            item.EndTime =
                new DateTimeOffset(
                        DateTime.Parse(WebUtility.HtmlDecode(datesRow.Descendants("td").Last().InnerText
                            .Replace("(Japan Time)", "").Trim())), TimeSpan.FromHours(9))
                    .ToUniversalTime().UtcDateTime;

            var conditionRow = detailsGrid.Descendants("tr").First(node => node.InnerText.Contains("Condition"));
            var condition = conditionRow.Descendants("td").First().InnerText.Trim();

            if (condition == "New")
                item.Condition = YahooItem.ItemCondition.New;
            else if (condition == "Used")
                item.Condition = YahooItem.ItemCondition.Used;

            var bidsRow = detailsGrid.Descendants("tr").First(node => node.InnerText.Contains("Current bids"));
            var bids = conditionRow.Descendants("td").First().InnerText.Trim();

            item.BidsCount = int.Parse(bids);

            output.Result = item;

            return Task.FromResult((ICrawlerResultSingle<YahooItem>) output);
        }

        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public class Item
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public int Price { get; set; }
            public string ImageUrl { get; set; }
            public string Url { get; set; }
            public int Sale { get; set; }
            public int Tax { get; set; }
            public int Postage { get; set; }
            public string Seller { get; set; }
            public string CategoryId { get; set; }
            public int Bids { get; set; }
            public string EndTime { get; set; }
            public int BuyItNowPrice { get; set; }
            public string Condition { get; set; }
            public int ExhibitType { get; set; }
            public bool IsReserved { get; set; }
            public bool IsNewArrival { get; set; }
            public bool IsCheck { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public class RootObject
        {
            public int Count { get; set; }
            public int Hits { get; set; }
            public List<Item> Items { get; set; }
        }
    }
}

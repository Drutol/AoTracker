using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using AoTracker.Crawlers.Abstract;
using AoTracker.Crawlers.Infrastructure;
using AoTracker.Crawlers.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AoTracker.Crawlers.Sites.Yahoo
{
    class YahooParser : TypedParser<YahooItem, YahooSourceParameters>
    {
        protected override Task<ICrawlerResult<YahooItem>> Parse(string data, YahooSourceParameters parameters)
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
                        .ToUniversalTime(),
                    IsShippingFree = rootItem.Postage == 0,
                    Condition = condition,
                    Tax = rootItem.Tax,
                    BidsCount = rootItem.Bids,
                };

                parsedItems.Add(item);
            }

            return Task.FromResult((ICrawlerResult<YahooItem>)output);
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

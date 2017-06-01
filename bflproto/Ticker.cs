using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace bflproto
{
    public class TickerData
    {
        [JsonProperty("product_code")]
        public string ProductCode { get; set; }
        [JsonProperty("best_bid")]
        public decimal BestBid { get; set; }
        [JsonProperty("best_ask")]
        public decimal BestAsk { get; set; }
    }

    public class Ticker
    {
        private static readonly Uri endpointUri = new Uri("https://api.bitflyer.jp");

        public static async Task<TickerData> Run()
        {
            var method = "GET";
            var path = "/v1/ticker";
            var query = "";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(new HttpMethod(method), path + query))
            {
                client.BaseAddress = endpointUri;
                var message = await client.SendAsync(request);
                var response = await message.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TickerData>(response);
            }
        }
    }
}

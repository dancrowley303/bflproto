using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace bflproto
{

    public class BalanceData
    {
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public decimal Available { get; set; }
    }

    public class Balance
    {
        private static readonly Uri endpointUri = new Uri("https://api.bitflyer.jp");

        public static async Task<List<BalanceData>> Run(string apiKey, string apiSecret)
        {
            var method = "GET";
            var path = "/v1/me/getbalance";
            var query = "";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(new HttpMethod(method), path + query))
            {
                client.BaseAddress = endpointUri;

                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                var data = timestamp + method + path + query;
                var hash = SignWithHMACSHA256(data, apiSecret);
                request.Headers.Add("ACCESS-KEY", apiKey);
                request.Headers.Add("ACCESS-TIMESTAMP", timestamp);
                request.Headers.Add("ACCESS-SIGN", hash);

                var message = await client.SendAsync(request);
                var response = await message.Content.ReadAsStringAsync();

                var balanceData = JsonConvert.DeserializeObject<List<BalanceData>>(response);
                return balanceData;
            }
        }

        private static string SignWithHMACSHA256(string data, string secret)
        {
            using (var encoder = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                var hash = encoder.ComputeHash(Encoding.UTF8.GetBytes(data));
                return ToHexString(hash);
            }
        }

        private static string ToHexString(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            foreach(var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}

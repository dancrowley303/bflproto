using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace bflproto
{

    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
                {
                    var apiKey = ConfigurationManager.AppSettings["apiKey"];
                    var apiSecret = ConfigurationManager.AppSettings["apiSecret"];

                    var ticker = await Ticker.Run();
                    Console.WriteLine("***Market Info***");
                    Console.WriteLine("Best Bid is {0}", ticker.BestBid);
                    Console.WriteLine("Best Ask is {0}", ticker.BestAsk);
                    Console.WriteLine("Spread is {0}", ticker.BestAsk - ticker.BestBid);
                    Console.WriteLine();

                    Console.WriteLine("***My Balances***");
                    var bd = await Balance.Run(apiKey, apiSecret);
                    foreach (var item in bd)
                    {
                        Console.WriteLine("{0} - Amount: {1}, Available: {2}", item.CurrencyCode, item.Amount, item.Available);
                    }
                    Console.WriteLine();
                }).GetAwaiter().GetResult();
        }
    }
}

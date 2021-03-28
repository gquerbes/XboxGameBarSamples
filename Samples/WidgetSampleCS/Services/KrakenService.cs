using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WidgetSampleCS.Services
{
    public class KrakenService
    {
        private const string _baseURL = "https://api.kraken.com/0";

        public async Task<Dictionary<string,Dictionary<string, string>>> GetTickerForCoins(List<string> coins)
        {
            //build URL
            var pairs = string.Empty;
            foreach (var coin in coins)
            {
                if (!string.IsNullOrEmpty(pairs))
                {
                    pairs += ",";
                }

                pairs += $"{coin.ToUpper()}USD";
            }
            var url = $"{_baseURL}/public/Ticker?pair={pairs}";


            try
            {
                var mainDictionary = new Dictionary<string, Dictionary<string, string>>();
                //query endpoint
                var response = await HttpClientService.Instance.GetAsync(url);

                //sucess
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());

                    foreach(var coin in coins)
                    {
                        var valueDict = new Dictionary<string, string>();
                        valueDict.Add("price", responseJson["result"][$"{coin}USD"]["c"].FirstOrDefault().ToString());
                        valueDict.Add("openPrice", responseJson["result"][$"{coin}USD"]["o"].ToString());
                        mainDictionary.Add(coin, valueDict);
                    }


                    return mainDictionary;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }



    }
}

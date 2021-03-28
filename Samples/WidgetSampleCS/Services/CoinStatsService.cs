using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WidgetSampleCS.Services
{
    public class CoinStatsService
    {
        private const string _baseURL = "https://api.coinstats.app/public/v1";


        public async Task<Dictionary<string, string>> GetTickerForCoin(string coin)
        {
            try
            {
                var dict = new Dictionary<string, string>();
                var url = $"{_baseURL}/coins/{coin.ToLower()}?currency=USD";
                var result = await HttpClientService.Instance.GetAsync(url);
                JObject value = JObject.Parse(await result.Content.ReadAsStringAsync());

                dict.Add("price", value["coin"]["price"].ToString());
                dict.Add("change", value["coin"]["priceChange1d"].ToString());

                return dict;
            }
            catch
            {
                return null;
            }
        }


    }
}

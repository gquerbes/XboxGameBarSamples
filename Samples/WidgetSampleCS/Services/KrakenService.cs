using Newtonsoft.Json;
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

        public async Task<string> GetTickerForCoin(string coin)
        {

            var url = $"{_baseURL}/public/Ticker?pair={coin}USD";
            var result = await HttpClientService.Instance.GetAsync(url);

            return result.ToString();
        }


        public async Task<KrakenTickerResponse> GetTicker(List<string> coins)
        {
            var pairs = string.Empty;
            foreach(var coin in coins)
            {
                pairs += $"{coin.ToUpper()}USD,";
            }
            var url = $"{_baseURL}/public/Ticker?pair={pairs}";

            try
            {
                var response = await HttpClientService.Instance.GetAsync("https://api.kraken.com/0/public/Ticker?pair=ADAUSD,BTCUSD");

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();

                    var value = JsonConvert.DeserializeObject<KrakenTickerResponse>(responseJson);
                    return value;
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace WidgetSampleCS.ViewModels
{
    public class WidgetViewModel : INotifyPropertyChanged
    {
        public WidgetViewModel()
        {
            _ctx = SynchronizationContext.Current;
            httpClient = new HttpClientService();
            UpdateTicker();
        }

        private SynchronizationContext _ctx;
        private TickerResponse _cryptoTicker = null;
        HttpClientService httpClient = null;

        public string Title => "🤚💎✋\nCrypto Tracker\n";

        public string ADAPrice => $"ADA: {FormatCurrency(_cryptoTicker?.result.ADAUSD.c.FirstOrDefault(), "6")}";
        public decimal ADAChange => CalculatePercentChange(_cryptoTicker?.result.ADAUSD.o, _cryptoTicker?.result.ADAUSD.c.FirstOrDefault());
        public string BTCPrice => $"BTC: {FormatCurrency(_cryptoTicker?.result.XXBTZUSD.c.FirstOrDefault(), "2")}";
        public decimal BTCChange => CalculatePercentChange(_cryptoTicker?.result.XXBTZUSD.o, _cryptoTicker?.result.XXBTZUSD.c.FirstOrDefault());


        private decimal CalculatePercentChange(string OpenPriceString, string CurrentPriceString)
        {
            var didParseOpen = decimal.TryParse(OpenPriceString, out var openPrice);
            var didParseCurrent = decimal.TryParse(CurrentPriceString, out var currentPrice);
            if(!didParseCurrent || !didParseOpen) { return 0; }

            var increase = currentPrice - openPrice ;

            //round to 2 decimal places
            return Math.Round((increase / openPrice) * 100, 2);
        }

        private string FormatCurrency(string value, string decimalPlaces)
        {
            var didParse = decimal.TryParse(value, out var val);
            if (!didParse) { return ""; }
            return val.ToString($"C{decimalPlaces}", CultureInfo.CurrentCulture);

        }

        public string GMEPrice => $"GME: 🚀🌙";

        public event PropertyChangedEventHandler PropertyChanged;


        
        private async void UpdateTicker()
        {
            do
            {
             _cryptoTicker = await GetCryptoTicker();
                _ctx.Post((state) => {
                    OnPropertyChanged(nameof(ADAPrice));
                    OnPropertyChanged(nameof(ADAChange));
                    OnPropertyChanged(nameof(BTCChange));
                    OnPropertyChanged(nameof(BTCPrice));

                }, null);
                Thread.Sleep(1000 * 3);
            
            } while (true);
        
        }

        private async Task<TickerResponse> GetCryptoTicker()
        {
            
            var response = await httpClient.GetAsync("https://api.kraken.com/0/public/Ticker?pair=ADAUSD,BTCUSD");

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();

                var value = JsonConvert.DeserializeObject<TickerResponse>(responseJson);
                return value;
            }
            else
            {
                throw new Exception();
            }

        }

        private async Task GetStockTicker()
        {

        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Globalization;
using WidgetSampleCS.Services;
using System.Collections.Generic;

namespace WidgetSampleCS.ViewModels
{
    public class WidgetViewModel : INotifyPropertyChanged
    {
        public WidgetViewModel()
        {
            _ctx = SynchronizationContext.Current;
            UpdateTicker();
        }

        private SynchronizationContext _ctx;
        private KrakenTickerResponse _cryptoTicker = null;
        KrakenService kraken = new KrakenService();
        CoinStatsService coinStats = new CoinStatsService();
        private List<string> krakenCoins = new List<string> {"ADA","BTC"};


        public bool Online { get; set; }

        public string ADAPrice => $"ADA: {FormatCurrency(_cryptoTicker?.result.ADAUSD.c.FirstOrDefault(), "6")}";
        public decimal ADAChange => CalculatePercentChange(_cryptoTicker?.result.ADAUSD.o, _cryptoTicker?.result.ADAUSD.c.FirstOrDefault());
        public string BTCPrice => $"BTC: {FormatCurrency(_cryptoTicker?.result.XXBTZUSD.c.FirstOrDefault(), "2")}";
        public decimal BTCChange => CalculatePercentChange(_cryptoTicker?.result.XXBTZUSD.o, _cryptoTicker?.result.XXBTZUSD.c.FirstOrDefault());

        private Dictionary<string,string> omi;
        public string OMIPrice => $"OMI: {FormatCurrency(omi?["price"], "8")}";
        public string OMIChange => $"{omi?["change"]}";

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




        
        private async void UpdateTicker()
        {
            do
            {
                //get coinstats coins
                omi = await coinStats.GetTickerForCoin("ECOMI");
                //get kraken coins
                _cryptoTicker = await kraken.GetTicker(krakenCoins);
                //update view
                _ctx.Post((state) => {
                    OnPropertyChanged(nameof(ADAPrice));
                    OnPropertyChanged(nameof(ADAChange));
                    OnPropertyChanged(nameof(BTCChange));
                    OnPropertyChanged(nameof(BTCPrice));
                    OnPropertyChanged(nameof(OMIPrice));
                    OnPropertyChanged(nameof(OMIChange));
                    OnPropertyChanged(nameof(Online));

                }, null);
                Thread.Sleep(1000 * 3);
            
            } while (true);
        
        }



        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion Property Changed
    }
}

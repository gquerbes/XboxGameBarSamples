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
        //private List<string> krakenCoins = new List<string> {"ADA","BTC"};
        private Dictionary<string, string> omiValues;
        private Dictionary<string, string> adaValues;
        private Dictionary<string, string> btcValues;

        public bool Online { get; set; }

        public string ADAPrice => $"ADA: {FormatCurrency(adaValues?["price"], "6")}";
        public string BTCPrice => $"BTC: {FormatCurrency(btcValues?["price"], "2")}";
        public string OMIPrice => $"OMI: {FormatCurrency(omiValues?["price"], "8")}";

        public decimal ADAChange => adaValues == null ? 0 : decimal.Parse(adaValues?["change"]);
        public decimal BTCChange => btcValues == null ? 0 : decimal.Parse(btcValues?["change"]);
        public decimal OMIChange => omiValues == null ? 0 : decimal.Parse(omiValues?["change"]);

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
                try
                {
                    //get coinstats coins
                    omiValues = await coinStats.GetTickerForCoin("ECOMI");
                    adaValues = await coinStats.GetTickerForCoin("CARDANO");
                    btcValues = await coinStats.GetTickerForCoin("BITCOIN");
                    Online = true;
                }
                catch
                {
                    Online = false;
                }

                
                //get kraken coins
                //_cryptoTicker = await kraken.GetTicker(krakenCoins);
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

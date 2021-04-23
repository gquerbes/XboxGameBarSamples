using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WidgetSampleCS
{
    public static class WebViewSettings
    {
        public delegate void HTMLValueChanged();
        public static HTMLValueChanged OnHTMLValueChanged;

        private const string _autoRefreshSetting = "AUTO_REFRESH";
        private const string _refreshIntervalSetting = "REFRESH_INTERVAL";
        private const string _htmlMarkup = "HTML_MARKUP";

        private static int? _refreshInterval;
        public static int RefreshInterval
        {
            get
            {
                //not set in memory
                if (_refreshInterval == null)
                {
                    //get from device settings
                    var savedValue = (int?)ApplicationData.Current.LocalSettings.Values[_refreshIntervalSetting];

                    _refreshInterval = savedValue;


                    //default to false if not set
                    if (_refreshInterval == null)
                    {
                        _refreshInterval = 0;
                    }

                }
                return (int)_refreshInterval;
            }

            set
            {
                if (value != _refreshInterval)
                {
                    ApplicationData.Current.LocalSettings.Values[_refreshIntervalSetting] = value;
                    _refreshInterval = value;
                }
            }
        }

        private static bool? _autoRefresh;
        public static bool AutoRefresh
        {
            get
            {
                //not set in memory
                if (_autoRefresh == null)
                {
                    //get from device settings
                    var savedValue = ApplicationData.Current.LocalSettings.Values[_autoRefreshSetting];

                    _autoRefresh = (bool?)savedValue;


                    //default to false if not set
                    if (_autoRefresh == null)
                    {
                        _autoRefresh = false;
                    }

                }
                return (bool)_autoRefresh;
            }

            set
            {
                if(value != _autoRefresh)
                {
                    ApplicationData.Current.LocalSettings.Values[_autoRefreshSetting] = value;
                    _autoRefresh = value;
                }
            }
        }



        private static string _html;
        public static string HTML
        {
            get
            {
                if (string.IsNullOrEmpty(_html))
                {
                    var savedValue = ApplicationData.Current.LocalSettings.Values[_htmlMarkup];

                    if(savedValue == null)
                    {
                        _html = $"<h1>Please configure settings<h1>";
                    }
                    else
                    {
                        _html = (string)savedValue;
                    }
                }
                return _html;
            }
            set
            {
                if(value != _html)
                {
                    _html = value;
                    OnHTMLValueChanged?.Invoke();
                }
            }
        }
    }
}

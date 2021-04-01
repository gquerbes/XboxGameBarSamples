using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WidgetSampleCS.ViewModels
{
    public class SettingsViewModel
    {

        public bool AutoRefresh
        {
            get => WebViewSettings.AutoRefresh;
            set => WebViewSettings.AutoRefresh = value;
        }

        public int RefreshInterval
        {
            get => WebViewSettings.RefreshInterval;
            set => WebViewSettings.RefreshInterval = value;
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

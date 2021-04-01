using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WidgetSampleCS.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WidgetSampleCS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsWidget : Page
    {
        private XboxGameBarWidget widget = null;
        public SettingsViewModel ViewModel { get; set; }

        public SettingsWidget()
        {
            this.InitializeComponent();
            ViewModel = new SettingsViewModel();
            DataContext = ViewModel;
            TextBox.TextDocument.SetText(Windows.UI.Text.TextSetOptions.None, WebViewSettings.HTML);
        }

      
    
        /// <summary>
        /// Handle done button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox.TextDocument.GetText(Windows.UI.Text.TextGetOptions.None,  out string val);
            WebViewSettings.HTML = val;
        }

    }
}

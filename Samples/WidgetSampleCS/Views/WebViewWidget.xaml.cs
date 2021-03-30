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
    public sealed partial class WebViewWidget : Page
    {
        private XboxGameBarWidget widget = null;
        public WebViewWidgetViewModel ViewModel { get; set; }

        public WebViewWidget()
        {
            this.InitializeComponent();
            ViewModel = new WebViewWidgetViewModel();
            DataContext = ViewModel;

            GlobalSettings.OnHTMLValueChanged += (() => ShowWebView());
        }

        public async void ShowWebView()
        {
            var html = GlobalSettings.HTML;
            if (string.IsNullOrEmpty(html))
            {
                html = "<h1>TEST</h1>";
            }

           await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                webview.NavigateToString(html);

            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            widget = e.Parameter as XboxGameBarWidget;
            widget.RequestedOpacityChanged += Widget_RequestedOpacityChanged;
            widget.SettingsClicked += Widget_SettingsClicked;

        }

     

        private async void Widget_SettingsClicked(XboxGameBarWidget sender, object args)
        {
            await sender.ActivateSettingsAsync();
        }

        private async void Widget_RequestedOpacityChanged(XboxGameBarWidget sender, object args)
        {
            await webview.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                SetBackgroundOpacity();
            });
        }

        private void SetBackgroundOpacity()
        {
            this.Opacity = widget.RequestedOpacity;
        }
    }
}

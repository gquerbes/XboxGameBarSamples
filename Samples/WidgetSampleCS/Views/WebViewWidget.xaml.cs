using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using WidgetSampleCS.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
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

            //load webview on launch of widget
            LoadWebViews();
            //init auto update
            AutoUpdate();

            primaryWebView.LoadCompleted += (x, y) =>
            {
                ToggleVisibilityWithDelay(8, Visibility.Visible);
            };


        }



        private async void AutoUpdate()
        {
            await Task.Run(() =>
            {
                do
                {
                    //wait for refresh interval period
                    var refeshInterval = WebViewSettings.RefreshInterval > 15 ? WebViewSettings.RefreshInterval : 15;
                    Thread.Sleep(refeshInterval * 1000);

                    //auto refresh view if enabled
                    if (WebViewSettings.AutoRefresh)
                    {
                        RefreshWebView(primaryWebView);
                    }

                } while (true);
            });


        }


        private async void ToggleVisibilityWithDelay(int delay, Visibility visibility)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await Task.Delay(delay * 1000);
                primaryWebView.Visibility = visibility;
                RefreshWebView(secondWebView);

            });
        }


        private async void RefreshWebView(WebView webview)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if(webview == primaryWebView)
                {
                    primaryWebView.Visibility = Visibility.Collapsed;
                }
                webview.Refresh();
            });
        }


        private async void LoadWebViews()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
             {
                 var html = WebViewSettings.HTML;
                 //load primary
                 primaryWebView.NavigateToString(html);
                 //load secondary
                 secondWebView.NavigateToString(html);
             });

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            widget = e.Parameter as XboxGameBarWidget;
            //handle opacity requests
            widget.RequestedOpacityChanged += Widget_RequestedOpacityChanged;
            //tie in settings button event
            widget.SettingsClicked += Widget_SettingsClicked;
            //register for changes in HTML
            WebViewSettings.OnHTMLValueChanged = () => LoadWebViews();

         
        }



        private async void Widget_SettingsClicked(XboxGameBarWidget sender, object args)
        {
            await sender.ActivateSettingsAsync();
        }

        private async void Widget_RequestedOpacityChanged(XboxGameBarWidget sender, object args)
        {
            await primaryWebView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
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

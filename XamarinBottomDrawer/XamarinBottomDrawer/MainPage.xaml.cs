using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinBottomDrawer
{
    public partial class MainPage : ContentPage
    {
        uint duration = 100;
        double openY = (Device.RuntimePlatform == "Android") ? 20 : 60;
        double lastPanY = 0;
        bool isBackdropTapEnabled = true;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (backdrop.Opacity == 0)
            {
                await OpenDrawer();
            }
            else
            {
                await CloseDrawer();
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (isBackdropTapEnabled)
            {
                await CloseDrawer();
            }
        }

        private async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                isBackdropTapEnabled = false;
                lastPanY = e.TotalY;

                if (e.TotalY > 0)
                {
                    bottomToolbar.TranslationY = openY + e.TotalY;
                }
            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                if (lastPanY < 110)
                {
                    await OpenDrawer();
                }
                else
                {
                    await CloseDrawer();
                }
                isBackdropTapEnabled = true;
            }
        }

        private async Task OpenDrawer()
        {
            await Task.WhenAll(
                    backdrop.FadeTo(1, length: duration),
                    bottomToolbar.TranslateTo(0, openY, length: duration, easing: Easing.SinIn)
                );
            backdrop.InputTransparent = false;
        }

        private async Task CloseDrawer()
        {
            await Task.WhenAll(
                    backdrop.FadeTo(0, length: duration),
                    bottomToolbar.TranslateTo(0, 260, length: duration, easing: Easing.SinIn)
                );
            backdrop.InputTransparent = true;
        }
    }
}

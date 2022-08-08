using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinBottomDrawer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BottomDrawer : ContentView
    {
        public static readonly BindableProperty OutProperty =
            BindableProperty.Create(nameof(Out), typeof(View), typeof(BottomDrawer), null);

        public View Out
        {
            get { return (View)GetValue(OutProperty); }
            set { SetValue(OutProperty, value); }
        }

        public static readonly BindableProperty InProperty =
            BindableProperty.Create(nameof(In), typeof(View), typeof(BottomDrawer), null);

        public View In
        {
            get { return (View)GetValue(InProperty); }
            set { SetValue(InProperty, value); }
        }

        private uint duration = 100;
        private double openY = (Device.RuntimePlatform == "Android") ? 20 : 60;
        private double lastPanY = 0;
        private bool isBackdropTapEnabled = true;


        public BottomDrawer()
        {
            InitializeComponent();

            inFrame.SetBinding(Frame.ContentProperty, new Binding(nameof(In), source: this));
            outFrame.SetBinding(Frame.ContentProperty, new Binding(nameof(Out), source: this));
        }

        public async void Open()
        {
            await OpenDrawer();
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
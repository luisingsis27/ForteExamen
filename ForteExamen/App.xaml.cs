using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForteExamen
{
    public partial class App : Application
    {
        public App()
        {
            Device.SetFlags(new[] { "SwipeView_Experimental" });
            InitializeComponent();

            MainPage = new Views.LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

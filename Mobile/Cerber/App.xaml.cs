using Cerber.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cerber
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new ServicesListPage();
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

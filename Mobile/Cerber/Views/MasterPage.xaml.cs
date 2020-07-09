using Cerber.Repository;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cerber.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        public MasterPage()
        {
            InitializeComponent();
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            var repository = DependencyService.Resolve<IRepository>();
            repository.UpdateToken(null);

            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
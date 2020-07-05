using Cerber.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cerber.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _viewModel => BindingContext as LoginViewModel;

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = new LoginViewModel(Navigation);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            _viewModel.GoToRegistrationCommand.Execute(null);
        }
    }
}
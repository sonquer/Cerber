using Cerber.Models;
using Cerber.Repository;
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

            BindingContext = new LoginViewModel(Navigation, DependencyService.Resolve<IRepository>());
        }

        private void Create_Account_Clicked(object sender, EventArgs e)
        {
            _viewModel.GoToRegistrationCommand.Execute(null);
        }

        private void Login_Clicked(object sender, EventArgs e)
        {
            _viewModel.LoginCommand.Execute(new LoginModel
            {
                Email = email.Text,
                Password = password.Text
            });
        }
    }
}
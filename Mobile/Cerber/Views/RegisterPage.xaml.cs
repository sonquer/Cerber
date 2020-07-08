using Cerber.Models;
using Cerber.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cerber.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private RegisterViewModel _viewModel => BindingContext as RegisterViewModel;

        public RegisterPage()
        {
            InitializeComponent();

            BindingContext = new RegisterViewModel(Navigation);
        }

        private void Register_Clicked(object sender, System.EventArgs e)
        {
            _viewModel.RegisterCommand.Execute(new RegisterModel
            {
                Email = email.Text,
                Password = password.Text,
                ConfirmPassword = confirmPassword.Text
            });
        }
    }
}
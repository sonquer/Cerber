using Cerber.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cerber.ViewModels
{
    class LoginViewModel
    {
        private readonly INavigation _navigation;

        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }

        public ICommand GoToRegistrationCommand => new Command(OnGoToRegistration);

        private void OnGoToRegistration()
        {
            _navigation.PushAsync(new RegisterPage());
        }
    }
}

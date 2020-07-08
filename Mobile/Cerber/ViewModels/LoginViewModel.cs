using Cerber.Models;
using Cerber.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Cerber.Client;
using System.Net.Http;
using System;
using Cerber.Repository;

namespace Cerber.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _loginError = "";

        public string LoginError
        {
            get => _loginError;
            set => SetProperty(ref _loginError, value);
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool LoginButtonIsEnabled
        {
            get => _isLoading == false;
        }

        private readonly INavigation _navigation;

        private readonly IRepository _repository;

        public LoginViewModel(INavigation navigation, IRepository repository)
        {
            _navigation = navigation;
            _repository = repository;
        }

        public ICommand GoToRegistrationCommand => new Command(OnGoToRegistration);

        private void OnGoToRegistration()
        {
            _navigation.PushAsync(new RegisterPage());
        }

        public ICommand LoginCommand => new Command<LoginModel>(async (model) => await OnLogin(model));

        public async Task OnLogin(LoginModel model)
        {
            LoginError = "";

            if (model.Email is null || model.Email.Length <= 3)
            {
                LoginError = "email is too short";
                return;
            }

            if (model.Password is null || model.Password.Length <= 3)
            {
                LoginError = "password is too short";
                return;
            }

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://cluster.cerber.space/gateway/accounts/")
            };

            var accountsClient = new AccountsClient(httpClient);

            try
            {
                IsLoading = true;

                var result = await accountsClient.AuthorizeAsync(new AccountDto()
                {
                    Email = model.Email,
                    Password = model.Password
                });

                _repository.UpdateToken(new Repository.Models.Token()
                {
                    TokenValue = result.Token,
                    ExpiresAt = result.ExpiresAt.ToString()
                });

                IsLoading = false;

                Application.Current.MainPage = new NavigationPage(new ServicesListPage());
            }
            catch (Exception)
            {
                IsLoading = false;
                LoginError = "Invalid email or password";
            }
        }
    }
}

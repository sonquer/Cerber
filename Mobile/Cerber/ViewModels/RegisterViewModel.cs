using Cerber.Client;
using Cerber.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cerber.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private string _registerError = "";

        public string RegisterError
        {
            get => _registerError;
            set => SetProperty(ref _registerError, value);
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool RegisterButtonIsEnabled
        {
            get => _isLoading == false;
        }

        private readonly INavigation _navigation;

        public RegisterViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }

        public ICommand RegisterCommand => new Command<RegisterModel>(async (model) => await OnRegistration(model));

        public async Task OnRegistration(RegisterModel model)
        {
            RegisterError = "";

            if (model.Email is null || model.Email.Length <= 3)
            {
                RegisterError = "email is too short";
                return;
            }

            if (model.Password is null || model.Password.Length <= 7)
            {
                RegisterError = "password is too short";
                return;
            }

            if (model.Password != model.ConfirmPassword)
            {
                RegisterError = "passwords are different";
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

                var result = await accountsClient.CreateAsync(new AccountDto()
                {
                    Email = model.Email,
                    Password = model.Password
                });

                await _navigation.PopAsync();

                IsLoading = false;
            }
            catch (Exception)
            {
                IsLoading = false;
                RegisterError = "Something went wrong. Please try again later.";
            }
        }
    }
}

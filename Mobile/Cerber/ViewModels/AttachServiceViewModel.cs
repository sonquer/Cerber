using Cerber.Client;
using Cerber.Models;
using Cerber.Repository;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cerber.ViewModels
{
    public class AttachServiceViewModel : ViewModelBase
    {
        private string _createError = "";

        public string CreateError
        {
            get => _createError;
            set => SetProperty(ref _createError, value);
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool CreateButtonIsEnabled
        {
            get => _isLoading == false;
        }

        private readonly INavigation _navigation;

        public AttachServiceViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }

        public ICommand SaveAvailabilityRecordCommand => new Command<CreateServiceModel>(async (model) => await OnSaveAvailabilityRecord(model));

        private async Task OnSaveAvailabilityRecord(CreateServiceModel model)
        {
            CreateError = "";

            if (model.Name is null || model.Name.Length <= 3)
            {
                CreateError = "name is too short";
                return;
            }

            if (model.Url is null || model.Url.Length <= 3)
            {
                CreateError = "url is too short";
                return;
            }

            IsLoading = true;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://cluster.cerber.space/gateway/availability/")
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DependencyService.Resolve<IRepository>().GetToken().TokenValue);

            var availabilityClient = new AvailabilityClient(httpClient);

            await availabilityClient.AvailabilityAsync(new AvailabilityRecordDto
            {
                Name = model.Name,
                Url = model.Url,
                LogLifetimeThresholdInHours = model.LifetimeInHours,
                ExpectedStatusCode = model.ExpectedStatusCode,
                ExpectedResponse = model.ExpectedResponse
            });

            IsLoading = false;

            await _navigation.PopAsync();
        }
    }
}

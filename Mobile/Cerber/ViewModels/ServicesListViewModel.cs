using System.Windows.Input;
using Cerber.Models;
using Cerber.Views;
using Xamarin.Forms;
using Cerber.Client;
using System.Collections.ObjectModel;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Cerber.Repository;

namespace Cerber.ViewModels
{
    public class ServicesListViewModel : ViewModelBase
    {
        private ObservableCollection<ServiceModel> _services = new ObservableCollection<ServiceModel>();

        public ObservableCollection<ServiceModel> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }

        private bool _isRefreshing = false;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private readonly INavigation _navigation;

        public ServicesListViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }

        public ICommand GoToDetailsCommand => new Command<ServiceModel>(OnGoToDetails);

        private void OnGoToDetails(ServiceModel item)
        {
            _navigation.PushAsync(new ServiceDetailsPage(item));
        }

        public ICommand GoToAttachServiceCommand => new Command(GoToAttachService);

        private void GoToAttachService()
        {
            _navigation.PushAsync(new AttachServicesPage());
        }

        public ICommand RefreshServicesCommand => new Command(async() => await RefreshServices());

        private async Task RefreshServices()
        {
            IsRefreshing = true;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://cluster.cerber.space/gateway/availability/")
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DependencyService.Resolve<IRepository>().GetToken().TokenValue);

            var availabilityClient = new AvailabilityClient(httpClient);

            var services = await availabilityClient.AvailabilityAllAsync();

            var serviceCollection = new ObservableCollection<ServiceModel>();
            foreach (var service in services)
            {
                serviceCollection.Add(new ServiceModel
                {
                    Id = service.Id,
                    Name = service.Name,
                    Image = service.Status == "ST_OK" ? "ok.png" : "error.png"
                });
            }

            Services = serviceCollection;

            IsRefreshing = false;
        }
    }
}

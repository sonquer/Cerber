using Cerber.Client;
using Cerber.Models;
using Cerber.Repository;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cerber.ViewModels
{
    public class ServiceDetailsViewModel : ViewModelBase
    {
        private string _name = "";

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _url = "";

        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }


        private string _expectedStatusCode = "200";

        public string ExpectedStatusCode
        {
            get => _expectedStatusCode;
            set => SetProperty(ref _expectedStatusCode, value);
        }

        private string _expectedResponse = "";

        public string ExpectedResponse
        {
            get => _expectedResponse;
            set => SetProperty(ref _expectedResponse, value);
        }

        private string _status = "";

        public string StatusImage
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private ObservableCollection<AvailabilityLogModel> _availabilityLogs = new ObservableCollection<AvailabilityLogModel>();

        public ObservableCollection<AvailabilityLogModel> AvailabilityLogs
        {
            get => _availabilityLogs;
            set => SetProperty(ref _availabilityLogs, value);
        }

        private readonly ServiceModel _serviceModel;

        public ServiceDetailsViewModel(ServiceModel serviceModel)
        {
            _serviceModel = serviceModel;
        }

        public ICommand LoadServiceDetails => new Command(async() => await OnLoadServiceDetails());

        private async Task OnLoadServiceDetails()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://cluster.cerber.space/gateway/availability/")
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DependencyService.Resolve<IRepository>().GetToken().TokenValue);

            var availabilityClient = new AvailabilityClient(httpClient);

            var details = await availabilityClient.Availability2Async(_serviceModel.Id);

            Name = details.Name;
            Url = details.Url;
            ExpectedResponse = details.ExpectedResponse;
            ExpectedStatusCode = details.ExpectedStatusCode.ToString();
            StatusImage = details.Status == "ST_OK" ? "heart_status_ok" : "heart_status_error";

            var logs = new ObservableCollection<AvailabilityLogModel>();
            foreach(var log in details.AvailabilityLogs.Take(20))
            {
                logs.Add(new AvailabilityLogModel
                {
                    LogCreatedAt = log.CreatedAt.ToString("[yyyy-MM-dd hh:mm:ss]"),
                    LogResponseTime = $"{log.ResponseTime}ms.",
                    StatusImage = log.StatusCode == details.ExpectedStatusCode 
                        && log.Body == details.ExpectedResponse 
                        ? "ok" 
                        : "error"
                });
            }

            AvailabilityLogs = logs;
        }
    }
}

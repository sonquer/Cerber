using System.Windows.Input;
using Cerber.Models;
using Cerber.Views;
using Xamarin.Forms;

namespace Cerber.ViewModels
{
    public class ServicesListViewModel
    {
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
    }
}

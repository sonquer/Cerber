using Cerber.Models;
using Cerber.ViewModels;
using Xamarin.Forms;

namespace Cerber.Views
{
    public partial class ServiceDetailsPage : ContentPage
    {
        private ServiceDetailsViewModel _viewModel => BindingContext as ServiceDetailsViewModel;

        public ServiceDetailsPage(ServiceModel serviceModel)
        {
            InitializeComponent();

            BindingContext = new ServiceDetailsViewModel(serviceModel);

            _viewModel.LoadServiceDetails.Execute(null);
        }
    }
}

using Cerber.Models;
using Xamarin.Forms;

namespace Cerber.Views
{
    public partial class ServiceDetailsPage : ContentPage
    {
        private readonly ServiceModel _serviceModel;

        public ServiceDetailsPage(ServiceModel serviceModel)
        {
            InitializeComponent();

            _serviceModel = serviceModel;
        }
    }
}

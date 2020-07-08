using Cerber.Models;
using Cerber.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cerber.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttachServicesPage : ContentPage
    {
        private AttachServiceViewModel _viewModel => BindingContext as AttachServiceViewModel;

        public AttachServicesPage()
        {
            InitializeComponent();

            BindingContext = new AttachServiceViewModel(Navigation);
        }

        private void Create_Service_Clicked(object sender, System.EventArgs e)
        {
            _viewModel.SaveAvailabilityRecordCommand.Execute(new CreateServiceModel
            {
                Name = name.Text,
                Url = url.Text,
                ExpectedResponse = expectedResponse.Text,
                ExpectedStatusCode = int.Parse(expectedStatusCode.Text ?? "200"),
                LifetimeInHours = int.Parse(logLifetimeInHours.Text ?? "1")
            });
        }
    }
}
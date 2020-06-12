using System.Collections.ObjectModel;
using Cerber.Models;
using Cerber.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cerber.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServicesListPage : ContentPage
    {
        public ObservableCollection<ServiceModel> Services { get; set; }

        private ServicesListViewModel _viewModel => BindingContext as ServicesListViewModel;

        public ServicesListPage()
        {
            InitializeComponent();

            BindingContext = new ServicesListViewModel(Navigation);

            Services = new ObservableCollection<ServiceModel>
            {
                new ServiceModel
                {
                    Id = "12345",
                    Name = "test",
                    Status = "OK"
                },
                new ServiceModel
                {
                    Id = "34345",
                    Name = "oki",
                    Status = "error"
                }
            };

            ServicesListView.ItemsSource = Services;
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }

            _viewModel.GoToDetailsCommand.Execute(e.Item as ServiceModel);

            ((ListView)sender).SelectedItem = null;
        }

        void Button_Clicked(object sender, System.EventArgs e)
        {
            _viewModel.GoToAttachServiceCommand.Execute(null);
        }
    }
}

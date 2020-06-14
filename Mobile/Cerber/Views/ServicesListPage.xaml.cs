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
                    Id = "1",
                    Name = "Main.Api",
                    Image = "ok.png"
                },
                new ServiceModel
                {
                    Id = "2",
                    Name = "Import.Api",
                    Image = "ok.png"
                },
                new ServiceModel
                {
                    Id = "3",
                    Name = "Search.Api",
                    Image = "error.png"
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

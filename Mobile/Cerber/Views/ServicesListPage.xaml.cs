using System.Collections.ObjectModel;
using Cerber.Models;
using Cerber.Repository;
using Cerber.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cerber.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServicesListPage : ContentPage
    {
        private ServicesListViewModel _viewModel => BindingContext as ServicesListViewModel;

        public ServicesListPage()
        {
            InitializeComponent();

            BindingContext = new ServicesListViewModel(Navigation);

            _viewModel.RefreshServicesCommand.Execute(null);
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

using Cerber.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cerber
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            Master = new MasterPage();
            Detail = new NavigationPage(new ServicesListPage());
        }
    }
}
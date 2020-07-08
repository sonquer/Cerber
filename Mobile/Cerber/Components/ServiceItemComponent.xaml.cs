using Xamarin.Forms;

namespace Cerber.Components
{
    public partial class ServiceItemComponent : StackLayout
    {
        public ServiceItemComponent()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty NameProperty = BindableProperty
            .Create(nameof(Name), typeof(string), typeof(ServiceItemComponent), null);

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public static readonly BindableProperty StatusProperty = BindableProperty
            .Create(nameof(Status), typeof(string), typeof(ServiceItemComponent), null);

        public string Status
        {
            get => (string)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }
    }
}

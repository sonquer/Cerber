using Autofac;
using Cerber.Repository;
using Cerber.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Cerber
{
    public partial class App : Application
    {
        static IContainer container;

        static readonly ContainerBuilder builder = new ContainerBuilder();

        public App()
        {
            InitializeComponent();

            RegisterSingletonType<IRepository, CerberDatabaseContext>();
            BuildContainer();

            DependencyResolver.ResolveUsing(type => container.IsRegistered(type) ? container.Resolve(type) : null);

            var repository = DependencyService.Resolve<IRepository>();
            if (repository.GetToken() != null)
            {
                MainPage = new NavigationPage(new ServicesListPage());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected static void RegisterType<TInterface, T>() where TInterface : class where T : class, TInterface
        {
            builder.RegisterType<T>().As<TInterface>();
        }

        protected static void RegisterSingletonType<TInterface, T>() where TInterface : class where T : class, TInterface
        {
            builder.RegisterType<T>().As<TInterface>().SingleInstance();
        }

        protected static void BuildContainer()
        {
            if (container is null)
            {
                container = builder.Build();
            }
        }
    }
}

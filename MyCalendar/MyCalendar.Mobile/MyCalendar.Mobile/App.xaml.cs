using MyCalendar.Mobile.Common.Constants;
using MyCalendar.Mobile.Common.Services;
using MyCalendar.Mobile.Common.Services.Appointment;
using MyCalendar.Mobile.Common.Services.Authentication;
using MyCalendar.Mobile.Views.Home;
using MyCalendar.Mobile.Views.Home.TabPages.AddAppointment;
using MyCalendar.Mobile.Views.Home.TabPages.Schedule;
using MyCalendar.Mobile.Views.Login;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MyCalendar.Mobile
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync(NavigationConstants.LoginPage);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegisterNavigation(containerRegistry);
            RegisterServices(containerRegistry);
        }

        private static void RegisterNavigation(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();

            containerRegistry.RegisterForNavigation<HomePage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<AddAppointmentPage, AddAppointmentPageViewModel>();
            containerRegistry.RegisterForNavigation<SchedulePage, SchedulePageViewModel>();
        }

        private static void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<AppSettingsManager>();
            containerRegistry.RegisterSingleton<IAuthenticationService, AuthenticationService>();
            containerRegistry.RegisterSingleton<IAppointmentService, AppointmentServiceMock>();
        }
    }
}

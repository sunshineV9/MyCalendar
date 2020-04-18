using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyCalendar.Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService dialogService;

        public ICommand LoginCommand { get; set; }

        public MainPageViewModel(
            IPageDialogService dialogService,
            INavigationService navigationService)
            : base(navigationService)
        {
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            this.Title = "Main Page";

            this.LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            try
            {
                var authResult = await WebAuthenticator.AuthenticateAsync(
                new Uri("http://10.0.2.2:5000/api/Account/login"),
                new Uri("myapp://"));

                var accessToken = authResult?.AccessToken;
            }
            catch (Exception ex)
            {
                await this.dialogService.DisplayAlertAsync("Login failed", "Error on login: " + ex.Message, "OK");
            }
        }
    }
}

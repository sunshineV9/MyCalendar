using MyCalendar.Mobile.Common.Services.Authentication;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyCalendar.Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IPageDialogService dialogService;

        private bool loggedIn;

        public bool LoggedIn
        {
            get { return loggedIn; }
            set { SetProperty(ref loggedIn, value); }
        }

        public ICommand LoginCommand { get; set; }

        public ICommand LogoutCommand { get; set; }

        public MainPageViewModel(
            IAuthenticationService authenticationService,
            IPageDialogService dialogService,
            INavigationService navigationService)
            : base(navigationService)
        {
            this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            this.Title = "Main Page";

            this.LoginCommand = new Command(async () => await LoginAsync());
            this.LogoutCommand = new Command(async () => await LogoutAsync());
        }

        private async Task LoginAsync()
        {
            try
            {
                var result = await this.authenticationService.LoginAsync();

                if (result != null)
                {
                    await SecureStorage.SetAsync("access_token", result);

                    this.LoggedIn = true;
                }
                else
                {
                    throw new AuthenticationException("Could not authenticate user");
                }
            }
            catch (Exception ex)
            {
                await this.dialogService.DisplayAlertAsync("Login failed", "Error on login: " + ex.Message, "OK");
            }
        }

        private async Task LogoutAsync()
        {
            try
            {
                var result = await this.authenticationService.LogoutAsync();

                if (result)
                {
                    SecureStorage.Remove("accessToken");

                    this.LoggedIn = false;
                }
                else
                {
                    throw new AuthenticationException("Could not logout user");
                }
            }
            catch (Exception ex)
            {
                await this.dialogService.DisplayAlertAsync("Logout failed", "Error on login: " + ex.Message, "OK");
            }
        }
    }
}

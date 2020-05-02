using MyCalendar.Mobile.Common.Constants;
using MyCalendar.Mobile.Common.Services.Authentication;
using MyCalendar.Mobile.Common.ViewModels;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyCalendar.Mobile.Views.Login
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IPageDialogService dialogService;

        public ICommand LoginCommand { get; set; }

        public LoginPageViewModel(
            IAuthenticationService authenticationService,
            IPageDialogService dialogService,
            INavigationService navigationService)
            : base(navigationService)
        {
            this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            this.LoginCommand = new Command(async () => await LoginUserAsync());
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            await base.InitializeAsync(parameters);

            await LoginUserAsync();
        }

        private async Task LoginUserAsync()
        {
            try
            {
                if (await this.authenticationService.LoginAsync())
                {
                    await NavigateToHomeAsync();
                }
            }
            catch (Exception ex)
            {
                await this.dialogService.DisplayAlertAsync("Login failed", "Error on login: " + ex.Message, "OK");
            }
        }

        private async Task NavigateToHomeAsync()
        {
            await this.NavigationService.NavigateAsync(NavigationConstants.HomePage);
        }
    }
}

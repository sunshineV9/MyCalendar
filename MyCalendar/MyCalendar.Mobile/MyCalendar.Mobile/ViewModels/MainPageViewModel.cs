using MyCalendar.Mobile.Common.Services;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyCalendar.Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService dialogService;

        private bool loggedIn;
        private readonly AppSettingsManager appSettingsManager;

        public bool LoggedIn
        {
            get { return loggedIn; }
            set { SetProperty(ref loggedIn, value); }
        }

        public ICommand LoginCommand { get; set; }

        public ICommand LogoutCommand { get; set; }

        public MainPageViewModel(
            AppSettingsManager appSettingsManager,
            IPageDialogService dialogService,
            INavigationService navigationService)
            : base(navigationService)
        {
            this.appSettingsManager = appSettingsManager ?? throw new ArgumentNullException(nameof(appSettingsManager));
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            this.Title = "Main Page";

            this.LoginCommand = new Command(async () => await LoginAsync());
            this.LogoutCommand = new Command(async () => await LogoutAsync());
        }

        private async Task LoginAsync()
        {
            try
            {
                var authResult = await WebAuthenticator.AuthenticateAsync(
                new Uri(this.appSettingsManager["Authorization:Address"] + this.appSettingsManager["Authorization:Endpoint"]),
                new Uri(this.appSettingsManager["Authorization:Callback"] + "://"));

                await SecureStorage.SetAsync("access_token", authResult?.AccessToken);
                await SecureStorage.SetAsync("token_type", authResult?.Get("token_type"));

                this.LoggedIn = true;
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
                var httpClientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                    {
                        return true;
                    },
                };

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(this.appSettingsManager["Authorization:Address"]);

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(await SecureStorage.GetAsync("token_type"), await SecureStorage.GetAsync("access_token"));

                    var req = new HttpRequestMessage(HttpMethod.Post, this.appSettingsManager["Authorization:Endpoint"]);

                    var resp = await client.SendAsync(req);

                    if (resp.IsSuccessStatusCode)
                    {
                        SecureStorage.Remove("accessToken");

                        this.LoggedIn = false;
                    }
                }
            }
            catch (Exception ex)
            {
                await this.dialogService.DisplayAlertAsync("Logout failed", "Error on login: " + ex.Message, "OK");
            }
        }
    }
}

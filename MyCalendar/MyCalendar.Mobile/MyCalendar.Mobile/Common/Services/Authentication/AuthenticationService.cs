using MyCalendar.Mobile.Common.Constants;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MyCalendar.Mobile.Common.Services.Authentication
{
    class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettingsManager appSettingsManager;

        public AuthenticationService(AppSettingsManager appSettingsManager)
        {
            this.appSettingsManager = appSettingsManager ?? throw new ArgumentNullException(nameof(appSettingsManager));
        }

        public async Task<bool> LoginAsync()
        {
            var domain = this.appSettingsManager["Authorization:Domain"];
            var clientId = this.appSettingsManager["Authorization:ClientId"];
            var endpoint = this.appSettingsManager["Authorization:LoginEndpoint"];
            var callback = this.appSettingsManager["Authorization:LoginCallback"];

            var url = $"https://{domain}/{string.Format(endpoint, clientId, callback)}";

            var authResult = await AuthenticateAsync(url, callback);

            if (!string.IsNullOrEmpty(authResult?.AccessToken))
            {
                await SecureStorage.SetAsync(StorageConstants.AccessToken, authResult?.AccessToken);
                return true;
            }

            return false;
        }

        public async Task<bool> LogoutAsync()
        {
            var domain = this.appSettingsManager["Authorization:Domain"];
            var clientId = this.appSettingsManager["Authorization:ClientId"];
            var endpoint = this.appSettingsManager["Authorization:LogoutEndpoint"];
            var callback = this.appSettingsManager["Authorization:LogoutCallback"];

            var url = $"https://{domain}/{string.Format(endpoint, clientId, callback)}";

            await AuthenticateAsync(url, callback);

            return SecureStorage.Remove(StorageConstants.AccessToken);
        }

        private Task<WebAuthenticatorResult> AuthenticateAsync(string url, string callback)
        {
            return WebAuthenticator.AuthenticateAsync(
                new Uri(url),
                new Uri(callback));
        }
    }
}

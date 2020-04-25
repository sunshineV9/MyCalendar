using MyCalendar.Mobile.Common.Constants;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MyCalendar.Mobile.Common.Services.Authentication
{
    class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettingsManager appSettingsManager;

        private readonly string domain;
        private readonly string clientId;

        public AuthenticationService(AppSettingsManager appSettingsManager)
        {
            this.appSettingsManager = appSettingsManager ?? throw new ArgumentNullException(nameof(appSettingsManager));

            this.domain = this.appSettingsManager["Authorization:Domain"];
            this.clientId = this.appSettingsManager["Authorization:ClientId"];
        }

        public async Task<bool> LoginAsync()
        {
            var endpoint = this.appSettingsManager["Authorization:LoginEndpoint"];
            var callback = this.appSettingsManager["Authorization:LoginCallback"];

            var url = $"https://{this.domain}/{string.Format(endpoint, this.clientId, callback)}";

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
            var endpoint = this.appSettingsManager["Authorization:LogoutEndpoint"];
            var callback = this.appSettingsManager["Authorization:LogoutCallback"];

            var url = $"https://{this.domain}/{string.Format(endpoint, this.clientId, callback)}";

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

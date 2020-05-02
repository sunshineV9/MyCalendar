using IdentityModel.OidcClient;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace MyCalendar.Mobile.Common.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
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
            var callbackUri = this.appSettingsManager["Authorization:LoginCallback"];

            var options = new OidcClientOptions
            {
                Authority = $"https://{this.domain}",
                ClientId = this.clientId,
                Scope = "openid profile email",
                RedirectUri = callbackUri,
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                Browser = new Browser()
            };

            var oidcClient = new OidcClient(options);
            var result = await oidcClient.LoginAsync();

            if (result.IsError)
            {
                throw new AuthenticationException(result.Error);
            }

            return true;
        }

        public async Task<bool> LogoutAsync()
        {
            var callbackUri = this.appSettingsManager["Authorization:LogoutCallback"];

            var options = new OidcClientOptions
            {
                Authority = $"https://{this.domain}",
                ClientId = this.clientId,
                Scope = "openid profile email",
                RedirectUri = callbackUri,
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                Browser = new Browser()
            };

            var oidcClient = new OidcClient(options);
            var result = await oidcClient.LogoutAsync(new LogoutRequest());

            if (result.IsError)
            {
                throw new AuthenticationException(result.Error);
            }

            return true;
        }
    }
}

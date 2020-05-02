using IdentityModel.OidcClient.Browser;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MyCalendar.Mobile.Common.Services
{
    public class Browser : IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(new Uri(options.StartUrl), new Uri("com.companyname.mycalendar://login"));
            return new BrowserResult()
            {
                Response = ParseAuthenticatorResult(authResult)
            };
        }

        string ParseAuthenticatorResult(WebAuthenticatorResult result)
        {
            bool first = true;
            var resultString = "com.companyname.mycalendar://login";

            foreach (var property in result.Properties)
            {
                if (first)
                {
                    resultString += $"#{property.Key}={property.Value}";
                    first = false;
                }
                else
                {
                    resultString += $"&{property.Key}={property.Value}";
                }
            }

            return resultString;
        }
    }
}

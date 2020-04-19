using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyCalendar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private const string callbackScheme = "com.companyname.mycalendar";

        private readonly ILoggerFactory logger;

        public AccountController(ILoggerFactory logger)
        {
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        [HttpGet("{scheme}")]
        public async Task Get([FromRoute]string scheme)
        {
            this.logger.CreateLogger($"login requested for scheme [{scheme}]...");

            var auth = await Request.HttpContext.AuthenticateAsync(scheme);

            if (!auth.Succeeded
                || auth?.Principal == null
                || !auth.Principal.Identities.Any(id => id.IsAuthenticated)
                || string.IsNullOrEmpty(auth.Properties.GetTokenValue("access_token")))
            {
                this.logger.CreateLogger($"user not authorized, challenging endpoint...");

                await Request.HttpContext.ChallengeAsync(scheme);
            }
            else
            {
                this.logger.CreateLogger($"user authorized, creating callback url...");

                var qs = new Dictionary<string, string>
                {
                    { "token_type", auth.Properties.GetTokenValue("token_type") },
                    { "access_token", auth.Properties.GetTokenValue("access_token") },
                    { "refresh_token", auth.Properties.GetTokenValue("refresh_token") ?? string.Empty },
                    { "expires", (auth.Properties.ExpiresUtc?.ToUnixTimeSeconds() ?? -1).ToString() }
                };

                var url = callbackScheme + "://#" + string.Join(
                    "&",
                    qs.Where(kvp => !string.IsNullOrEmpty(kvp.Value) && kvp.Value != "-1")
                    .Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

                Request.HttpContext.Response.Redirect(url);
            }

            this.logger.CreateLogger($"login successfully processed for scheme [{scheme}]");
        }

        [Authorize]
        [HttpPost("{scheme}")]
        public async Task Logout([FromRoute]string scheme)
        {
            this.logger.CreateLogger($"logout requested for scheme [{scheme}]");

            await this.HttpContext.SignOutAsync(scheme, new AuthenticationProperties
            {
                RedirectUri = callbackScheme + "://"
            });

            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            this.logger.CreateLogger($"logout successfully processed for scheme [{scheme}]");
        }
    }
}
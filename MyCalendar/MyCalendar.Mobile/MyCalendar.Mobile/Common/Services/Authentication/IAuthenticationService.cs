using System.Threading.Tasks;

namespace MyCalendar.Mobile.Common.Services.Authentication
{
    /// <summary>
    /// Authentication service for handling login and logout of the user
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticates the user with the login endpoint from the backend
        /// </summary>
        /// <returns>True if success, otherwise false</returns>
        public Task<bool> LoginAsync();

        /// <summary>
        /// Calls the logout endpoint from the backend
        /// </summary>
        /// <returns>True if success, otherwise false</returns>
        public Task<bool> LogoutAsync();
    }
}

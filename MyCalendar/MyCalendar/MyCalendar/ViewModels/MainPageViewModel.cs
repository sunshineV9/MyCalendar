using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyCalendar.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ICommand LogInCommand { get; set; }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.Title = "Main Page";

            this.LogInCommand = new Command(async () => await LogIn());
        }

        private async Task LogIn()
        {
            var authResult = await WebAuthenticator.AuthenticateAsync(
                new Uri("https://mysite.com/mobileauth/Microsoft"),
                new Uri("myapp://"));

            var accessToken = authResult?.AccessToken;
        }
    }
}

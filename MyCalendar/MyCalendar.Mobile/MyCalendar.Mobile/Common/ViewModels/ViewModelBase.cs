using Prism.Mvvm;
using Prism.Navigation;
using System.Threading.Tasks;

namespace MyCalendar.Mobile.Common.ViewModels
{
    public class ViewModelBase : BindableBase, IInitializeAsync, INavigationAware, IDestructible
    {
        private string _title;

        protected INavigationService NavigationService { get; }

        public string Title { get => _title; set => SetProperty(ref _title, value); }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual Task InitializeAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }
    }
}

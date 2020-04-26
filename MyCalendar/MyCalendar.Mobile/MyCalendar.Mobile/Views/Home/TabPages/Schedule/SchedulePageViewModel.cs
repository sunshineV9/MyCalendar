using MyCalendar.Mobile.Common.Services.Appointment;
using MyCalendar.Mobile.Common.ViewModels;
using MyCalendar.Mobile.Views.Home.TabPages.AddAppointment;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCalendar.Mobile.Views.Home.TabPages.Schedule
{
    public class SchedulePageViewModel : ViewModelBase
    {
        private readonly IAppointmentService appointmentService;

        private ObservableCollection<AppointmentViewModel> appointments;
        private readonly IPageDialogService dialogService;

        public ObservableCollection<AppointmentViewModel> Appointments { get => appointments; set => SetProperty(ref appointments, value); }

        public ICommand RefreshCommand { get; }

        public SchedulePageViewModel(
            IAppointmentService appointmentService,
            IPageDialogService dialogService,
            INavigationService navigationService) : base(navigationService)
        {
            this.appointmentService = appointmentService ?? throw new System.ArgumentNullException(nameof(appointmentService));
            this.dialogService = dialogService ?? throw new System.ArgumentNullException(nameof(dialogService));

            this.RefreshCommand = new DelegateCommand(async () => await GetAppointmentsAsync());
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            await base.InitializeAsync(parameters);

            this.IsBusy = true;

            await GetAppointmentsAsync();
        }

        private async Task GetAppointmentsAsync()
        {
            try
            {
                this.Appointments = new ObservableCollection<AppointmentViewModel>(await this.appointmentService.GetAppointmentOverviewAsync());
                this.IsBusy = false;
            }
            catch (System.Exception ex)
            {
                this.IsBusy = false;
                await this.dialogService.DisplayAlertAsync("Failed to get Appointments", "Error on fetching appointments: " + ex.Message, "OK");
            }
        }
    }
}

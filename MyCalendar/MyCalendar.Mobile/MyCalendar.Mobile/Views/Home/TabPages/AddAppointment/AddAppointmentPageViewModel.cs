using MyCalendar.Mobile.Common.Services.Appointment;
using MyCalendar.Mobile.Common.ViewModels;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCalendar.Mobile.Views.Home.TabPages.AddAppointment
{
    public class AddAppointmentPageViewModel : ViewModelBase
    {
        private readonly IAppointmentService appointmentService;
        private readonly IPageDialogService dialogService;

        private bool allDay;

        private string location;

        private DateTime startTime;
        private DateTime endTime;

        public bool AllDay { get => allDay; set => SetProperty(ref allDay, value); }

        public string Location { get => location; set => SetProperty(ref location, value); }

        public DateTime StartTime { get => startTime; set => SetProperty(ref startTime, value); }

        public DateTime EndTime { get => endTime; set => SetProperty(ref endTime, value); }

        public ICommand SubmitCommand { get; }

        public AddAppointmentPageViewModel(
            IAppointmentService appointmentService,
            IPageDialogService dialogService,
            INavigationService navigationService)
            : base(navigationService)
        {
            this.appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            ClearInputs();

            this.SubmitCommand = new DelegateCommand(async () => await SubmitAppointmentAsync(), CanSubmit).ObservesProperty(() => this.Title);
        }

        private async Task SubmitAppointmentAsync()
        {
            this.IsBusy = true;

            try
            {
                var appointment = new AppointmentViewModel
                {
                    Title = this.Title,
                    Location = this.Location,
                    AllDay = this.AllDay,
                    StartTime = this.StartTime,
                    EndTime = this.EndTime,
                };

                await this.appointmentService.CreateAppointmentAsync(appointment);

                ClearInputs();
                this.IsBusy = false;
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                await this.dialogService.DisplayAlertAsync("Could not create appointment", "Error on creating appointment: " + ex.Message, "OK");
            }
        }

        private void ClearInputs()
        {
            this.Title = string.Empty;
            this.Location = string.Empty;
            this.AllDay = false;
            this.StartTime = DateTime.Now;
            this.EndTime = this.StartTime.AddHours(1);
        }

        private bool CanSubmit()
        {
            return !string.IsNullOrEmpty(this.Title);
        }
    }
}

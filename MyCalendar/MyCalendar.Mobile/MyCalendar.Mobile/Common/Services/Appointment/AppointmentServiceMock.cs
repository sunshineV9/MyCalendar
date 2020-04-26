using MyCalendar.Mobile.Views.Home.TabPages.AddAppointment;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCalendar.Mobile.Common.Services.Appointment
{
    public class AppointmentServiceMock : IAppointmentService
    {
        private List<AppointmentViewModel> appointments;

        public AppointmentServiceMock()
        {
            this.appointments = new List<AppointmentViewModel>();
        }

        public Task<int> CreateAppointmentAsync(AppointmentViewModel appointment)
        {
            appointment.Id = this.appointments.Count + 1;

            this.appointments.Add(appointment);

            return Task.FromResult(appointment.Id);
        }

        public Task<IEnumerable<AppointmentViewModel>> GetAppointmentOverviewAsync()
        {
            return Task.FromResult(this.appointments.AsEnumerable());
        }
    }
}

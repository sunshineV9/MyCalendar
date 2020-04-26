using MyCalendar.Mobile.Views.Home.TabPages.AddAppointment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCalendar.Mobile.Common.Services.Appointment
{
    /// <summary>
    /// Public API for managing appointments
    /// </summary>
    public interface IAppointmentService
    {
        /// <summary>
        /// Creates new appointment
        /// </summary>
        /// <param name="appointment">Appointment to be created</param>
        /// <returns>Id of new appointment</returns>
        public Task<int> CreateAppointmentAsync(AppointmentViewModel appointment);

        /// <summary>
        /// Gets all appointments for the current user
        /// </summary>
        /// <returns>List of appointments</returns>
        public Task<IEnumerable<AppointmentViewModel>> GetAppointmentOverviewAsync();
    }
}

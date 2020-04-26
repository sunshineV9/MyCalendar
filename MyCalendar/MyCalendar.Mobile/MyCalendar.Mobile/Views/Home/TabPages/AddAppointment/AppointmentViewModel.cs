using System;

namespace MyCalendar.Mobile.Views.Home.TabPages.AddAppointment
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        public bool AllDay { get; set; }

        public string Title { get; set; }

        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is AppointmentViewModel model &&
                   this.Id == model.Id &&
                   this.AllDay == model.AllDay &&
                   this.Title == model.Title &&
                   this.Location == model.Location &&
                   this.StartTime == model.StartTime &&
                   this.EndTime == model.EndTime;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.AllDay, this.Title, this.Location, this.StartTime, this.EndTime);
        }
    }
}

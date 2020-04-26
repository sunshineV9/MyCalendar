using Moq;
using MyCalendar.Mobile.Common.Services.Appointment;
using MyCalendar.Mobile.Views.Home.TabPages.AddAppointment;
using MyCalendar.Mobile.Views.Home.TabPages.Schedule;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MyCalendar.Mobile.Test.ViewModels
{
    public class SchedulePageViewModelTests
    {
        private SchedulePageViewModel viewModel;

        private Mock<IAppointmentService> appointmentServiceMock;
        private Mock<IPageDialogService> dialogServiceMock;

        public SchedulePageViewModelTests()
        {
            this.appointmentServiceMock = new Mock<IAppointmentService>();
            this.dialogServiceMock = new Mock<IPageDialogService>();
            var navigationServiceMock = new Mock<INavigationService>();

            this.viewModel = new SchedulePageViewModel(
                this.appointmentServiceMock.Object,
                this.dialogServiceMock.Object,
                navigationServiceMock.Object);
        }

        [Fact]
        public async Task Should_Retrieve_Appointments_From_AppointmentService()
        {
            // Arrange
            var appointments = new List<AppointmentViewModel>
            {
                GenerateAppointment(),
                GenerateAppointment(),
                GenerateAppointment(),
            };

            this.appointmentServiceMock
                .Setup(m => m.GetAppointmentOverviewAsync())
                .ReturnsAsync(appointments);

            // Act
            await this.viewModel.InitializeAsync(new NavigationParameters());

            // Assert
            this.appointmentServiceMock.Verify(m => m.GetAppointmentOverviewAsync(), Times.Once());
            Assert.Equal(appointments, this.viewModel.Appointments);
        }

        [Fact]
        public async Task Should_Handle_Exception_From_AppointmentService()
        {
            // Arrange
            var ex = new Exception();

            this.appointmentServiceMock
                .Setup(m => m.GetAppointmentOverviewAsync())
                .ThrowsAsync(ex);

            // Act
            await this.viewModel.InitializeAsync(new NavigationParameters());

            // Assert
            this.dialogServiceMock.Verify(m => m.DisplayAlertAsync("Failed to get Appointments", "Error on fetching appointments: " + ex.Message, "OK"), Times.Once());
        }

        private AppointmentViewModel GenerateAppointment()
        {
            return new AppointmentViewModel
            {
                Title = It.IsAny<string>(),
                Location = It.IsAny<string>(),
                AllDay = It.IsAny<bool>(),
                StartTime = It.IsAny<DateTime>(),
                EndTime = It.IsAny<DateTime>(),
            };
        }
    }
}

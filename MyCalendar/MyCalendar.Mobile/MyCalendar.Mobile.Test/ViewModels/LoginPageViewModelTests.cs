using Moq;
using MyCalendar.Mobile.Common.Constants;
using MyCalendar.Mobile.Common.Services.Authentication;
using MyCalendar.Mobile.Views.Login;
using Prism.Navigation;
using Prism.Services;
using System;
using Xunit;

namespace MyCalendar.Mobile.Test.ViewModels
{
    public class LoginPageViewModelTests
    {
        private LoginPageViewModel viewModel;

        private Mock<IAuthenticationService> authenticationServiceMock;
        private Mock<IPageDialogService> dialogServiceMock;
        private Mock<INavigationService> navigationServiceMock;

        public LoginPageViewModelTests()
        {
            this.authenticationServiceMock = new Mock<IAuthenticationService>();
            this.dialogServiceMock = new Mock<IPageDialogService>();
            this.navigationServiceMock = new Mock<INavigationService>();

            this.viewModel = new LoginPageViewModel(
                this.authenticationServiceMock.Object,
                this.dialogServiceMock.Object,
                this.navigationServiceMock.Object);
        }

        [Fact]
        public void Should_Login_User_On_Login_Button_Clicked()
        {
            // Arrange
            this.authenticationServiceMock
                .Setup(m => m.LoginAsync())
                .ReturnsAsync(true);

            // Act
            this.viewModel.LoginCommand.Execute(null);

            // Assert
            this.authenticationServiceMock.Verify(m => m.LoginAsync(), Times.Once());
        }

        [Fact]
        public void Should_Redirect_User_After_Login_To_HomePage()
        {
            // Arrange
            this.authenticationServiceMock
                .Setup(m => m.LoginAsync())
                .ReturnsAsync(true);

            // Act
            this.viewModel.LoginCommand.Execute(null);

            // Assert
            this.navigationServiceMock.Verify(m => m.NavigateAsync(NavigationConstants.HomePage), Times.Once());
        }

        [Fact]
        public void Should_Handle_Exception_On_Login()
        {
            // Arrange
            var ex = new Exception();

            this.authenticationServiceMock
                .Setup(m => m.LoginAsync())
                .ThrowsAsync(ex);

            // Act
            this.viewModel.LoginCommand.Execute(null);

            // Assert
            this.dialogServiceMock.Verify(m => m.DisplayAlertAsync("Login failed", "Error on login: " + ex.Message, "OK"), Times.Once());
        }
    }
}

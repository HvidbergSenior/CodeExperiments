using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Fakes;
using Insight.Services.EmailSender.Service;
using Insight.UserAccess.Application.RegisterUser;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Insight.UserAccess.Test.RegisterUser
{
    public class RegisterUserTests
    {
        [Fact]
        public async Task WhenCreatingUser_UserIsStored()
        {
            var fakeUnitOfWork = new FakeUnitOfWork();
            var fakeExecutionContext = new FakeExecutionContext();
            var mockSendEmail = new Mock<IEmailSender>();
            var mockOptions = Options.Create(new UserResetPasswordOptions());
            mockSendEmail.Setup(m => m.SendEmailAsync(It.IsAny<EmailMessage>())).ReturnsAsync(true);

            var userName = Any.UserName();
            var email = Any.Email();
            var password = Any.Password();
            const UserRole role = UserRole.User;
            var firstName = Any.FirstName();
            var lastName = Any.LastName();
            var customerPermissions = new List<CustomerPermissionGroup>();
            const UserStatus userStatus = UserStatus.Active;

            var cmd = RegisterUserCommand.Create(userName, email, password, password, role, customerPermissions, firstName, lastName, userStatus);

            var userList = new List<ApplicationUser>();

            var userManagerMock = Any.TestUserManager<ApplicationUser>();

            userManagerMock.Setup(c => c.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Callback((ApplicationUser user, string p) => userList.Add(user))
                .Returns(Task.FromResult(IdentityResult.Success));

            var userManager = userManagerMock.Object;
            var roleManager = Any.TestRoleManager<ApplicationRole>();

            var handler = new RegisterUserCommandHandler(userManager, fakeUnitOfWork, fakeExecutionContext, mockSendEmail.Object, mockOptions);

            await handler.Handle(cmd, CancellationToken.None);
            fakeUnitOfWork.IsCommitted.Should().BeTrue();
            mockSendEmail.Verify(m => m.SendEmailAsync(It.IsAny<EmailMessage>()), Times.Once);
            userList.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WhenCreatingUser_WhenUserAlreadyExists_UserIsNotStored()
        {
            var fakeUnitOfWork = new FakeUnitOfWork();
            var fakeExecutionContext = new FakeExecutionContext();
            var mockSendEmail = new Mock<IEmailSender>();
            var mockOptions = new Mock<IOptions<UserResetPasswordOptions>>();
            var userName = Any.UserName();
            var email = Any.Email();
            var password = Any.Password();
            var role = UserRole.User;
            var customerPermissions = new List<CustomerPermissionGroup>();
            var firstName = Any.FirstName();
            var lastName = Any.LastName();
            const UserStatus userStatus = UserStatus.Active;

            var cmd = RegisterUserCommand.Create(userName, email, password, password, role, customerPermissions, firstName, lastName, userStatus);

            var userList = new List<ApplicationUser>();

            var userManagerMock = Any.TestUserManager<ApplicationUser>();

            userManagerMock.Setup(c => c.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(Any.ApplicationUser());

            var userManager = userManagerMock.Object;

            var handler = new RegisterUserCommandHandler(userManager, fakeUnitOfWork, fakeExecutionContext, mockSendEmail.Object, mockOptions.Object);

            Func<Task> handle = async () => await handler.Handle(cmd, CancellationToken.None);

            await handle.Should().ThrowAsync<AlreadyExistingException>();
            
            fakeUnitOfWork.IsCommitted.Should().BeFalse();
            mockSendEmail.Verify(m => m.SendEmailAsync(It.IsAny<EmailMessage>()), Times.Never);
            userList.Should().BeEmpty();
        }
    }
}
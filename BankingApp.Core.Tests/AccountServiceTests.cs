using BankingApp.Core.AccountServices;
using BankingApp.DataRepository.Repositories;
using BankingApp.DataRepository.UnitOfWork;
using BankingApp.Models;
using BankingApp.ViewModels;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace BankingApp.Core.Tests
{
    public class AccountServiceTests
    {
        [Test]
        public void EncryptPassword_StringInput_PasswordEncrypted()
        {
            var _mock = new Mock<IUnitOfWorkFactory>();
            var mockUnit = new Mock<IUnitOfWork>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var mockSettings = new Mock<IAppSettings>();
            var accountService = new AccountService(_mock.Object,mockSettings.Object);
            var password = "EnCryPtME";

            var result = accountService.EncryptPassword(password);

            Assert.AreNotEqual(password, result);
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Never());
        }
        
        [Test]
        public void CreateToken_UserDataInput_TokenReturned()
        {
            var _mock = new Mock<IUnitOfWorkFactory>();
            var mockUnit = new Mock<IUnitOfWork>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var mockSettings = new Mock<IAppSettings>();
            mockSettings.Setup(s => s["key"]).Returns("h3a4t6e4o1rrrrrr3334444");
            var accountService = new AccountService(_mock.Object, mockSettings.Object);
            var user = new User { Id = 5, Name = "Spiidi" };

            string result = accountService.CreateToken(user);

            Assert.That(2, Is.EqualTo(result.Count(c => c == '.')));
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Never());
        }

        [Test]
        public void Register_DuplicateUsername_ErrorReturned()
        {
            var user = new User { Id = 1, Name = "Vito", Balance = 25000 };
            var registerModel = new RegisterViewModel { Name = "Vito", Password = "Umbembe2" };                                   
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetUserByName("Vito")).Returns(user);
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var mockSettings = new Mock<IAppSettings>();
            mockSettings.Setup(s => s["key"]).Returns("h3a4t6e4o1rrrrrr3334444");
            var accountService = new AccountService(_mock.Object, mockSettings.Object);

            var result = accountService.Register(registerModel);

            Assert.AreEqual(false, result.success);
            Assert.AreEqual("User already exists", result.message);
            Assert.AreEqual(null, result.responseContent);
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockRepo.Verify(r => r.GetUserByName(It.IsAny<string>()), Times.Once());
            mockUnit.Verify(u => u.Users.Create(It.IsAny<User>()), Times.Never());
            mockUnit.Verify(u => u.Save(), Times.Never());
        }

        [Test]
        public void Register_ValidData_RegistrationSuccessful()
        {
            var registerModel = new RegisterViewModel { Name = "Pascoal", Password = "Ukuleke" };
            var mockRepo = new Mock<IUserRepository>();
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var mockSettings = new Mock<IAppSettings>();
            mockSettings.Setup(s => s["key"]).Returns("h3a4t6e4o1rrrrrr3334444");
            var accountService = new AccountService(_mock.Object, mockSettings.Object);

            var result = accountService.Register(registerModel);

            Assert.AreEqual(true, result.success);
            Assert.AreEqual("Registration successfully completed", result.message);
            Assert.AreNotEqual(null, result.responseContent);
            Assert.That(2, Is.EqualTo(result.responseContent.Count(c => c == '.')));
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockRepo.Verify(r => r.GetUserByName(It.IsAny<string>()), Times.Once());            
            mockUnit.Verify(u => u.Save(), Times.Once());
        }

        [Test]
        public void Login_InvalidUsername_ErrorReturned()
        {
            var loginModel = new LoginViewModel { Name = "Pascoall", Password = "Ukuleke" };
            var mockRepo = new Mock<IUserRepository>();
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var mockSettings = new Mock<IAppSettings>();
            mockSettings.Setup(s => s["key"]).Returns("h3a4t6e4o1rrrrrr3334444");
            var accountService = new AccountService(_mock.Object, mockSettings.Object);

            var result = accountService.Login(loginModel);

            Assert.AreEqual(false, result.success);
            Assert.AreEqual("User not found", result.message);
            Assert.AreEqual(null, result.responseContent);            
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockRepo.Verify(r => r.GetUserByName(It.IsAny<string>()), Times.Once());            
        }

        [Test]
        public void Login_InvalidPassword_ErrorReturned()
        {
            var loginModel = new LoginViewModel { Name = "TeamNorth", Password = "HearTheRoarR" };
            var user = new User { Id = 6, Name = "TeamNorth", Password = "i??H?????????????|v?o?I??y??", Balance = 0 };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetUserByName("TeamNorth")).Returns(user);
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var mockSettings = new Mock<IAppSettings>();
            mockSettings.Setup(s => s["key"]).Returns("h3a4t6e4o1rrrrrr3334444");
            var accountService = new AccountService(_mock.Object, mockSettings.Object);

            var result = accountService.Login(loginModel);

            Assert.AreEqual(false, result.success);
            Assert.AreEqual("Password is wrong", result.message);
            Assert.AreEqual(null, result.responseContent);
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockRepo.Verify(r => r.GetUserByName(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Login_ValidUserData_LoginSuccessful()
        {
            var loginModel = new LoginViewModel { Name = "TeamNorth", Password = "HearTheRoar" };
            var user = new User { Id = 6, Name = "TeamNorth", Password = "i??H?????????????|v?o?I??y??", Balance = 0 };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetUserByName("TeamNorth")).Returns(user);
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var mockSettings = new Mock<IAppSettings>();
            mockSettings.Setup(s => s["key"]).Returns("h3a4t6e4o1rrrrrr3334444");
            var accountService = new AccountService(_mock.Object, mockSettings.Object);

            var result = accountService.Login(loginModel);

            Assert.AreEqual(true, result.success);
            Assert.AreEqual("Login successful", result.message);
            Assert.AreNotEqual(null, result.responseContent);
            Assert.That(2, Is.EqualTo(result.responseContent.Count(c => c == '.')));
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockRepo.Verify(r => r.GetUserByName(It.IsAny<string>()), Times.Once());
        }
    }
}

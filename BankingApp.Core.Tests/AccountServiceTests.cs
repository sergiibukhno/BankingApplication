using BankingApp.Core.AccountServices;
using BankingApp.Core.AppSettings;
using BankingApp.Core.TokenHandler;
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
            var _mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(mockUnitOfWork.Object);
            var mockTokenHandler = new Mock<ITokenHandler>();
            var accountService = new AccountService(_mockUnitOfWorkFactory.Object, mockTokenHandler.Object);
            var password = "EnCryPtME";

            var result = accountService.EncryptPassword(password);

            Assert.AreNotEqual(password, result);
            _mockUnitOfWorkFactory.Verify(mock => mock.GetUnitOfWork(), Times.Never());
        }

        [Test]
        public void Register_DuplicateUsername_ErrorReturned()
        {
            var user = new User { Id = 1, Name = "Vito", Balance = 25000 };
            var registerModel = new RegisterViewModel { Name = "Vito", Password = "Umbembe2" };                                   
            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(r => r.GetUserByName("Vito")).Returns(user);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.Users).Returns(mockUserRepo.Object);
            var _mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            _mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(mockUnitOfWork.Object);
            var mockSettings = new Mock<IAppSettings>();
            mockSettings.Setup(s => s["key"]).Returns("h3a4t6e4o1rrrrrr3334444");
            var mockTokenHandler = new Mock<ITokenHandler>();
            var accountService = new AccountService(_mockUnitOfWorkFactory.Object, mockTokenHandler.Object);

            var result = accountService.Register(registerModel);

            Assert.AreEqual(false, result.success);
            Assert.AreEqual("User already exists", result.message);
            Assert.AreEqual(null, result.responseContent);
            _mockUnitOfWorkFactory.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockUserRepo.Verify(r => r.GetUserByName(It.IsAny<string>()), Times.Once());
            mockUnitOfWork.Verify(u => u.Users.Create(It.IsAny<User>()), Times.Never());
            mockUnitOfWork.Verify(u => u.Save(), Times.Never());
        }

        [Test]
        public void Register_ValidData_RegistrationSuccessful()
        {
            var registerModel = new RegisterViewModel { Name = "Pascoal", Password = "Ukuleke" };
            var mockUserRepo = new Mock<IUserRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.Users).Returns(mockUserRepo.Object);
            var _mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            _mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(mockUnitOfWork.Object);
            var mockSettings = new Mock<IAppSettings>();
            mockSettings.Setup(s => s["key"]).Returns("h3a4t6e4o1rrrrrr3334444");
            var mockTokenHandler = new Mock<ITokenHandler>();
            var accountService = new AccountService(_mockUnitOfWorkFactory.Object, mockTokenHandler.Object);

            var result = accountService.Register(registerModel);

            Assert.AreEqual(true, result.success);
            Assert.AreEqual("Registration successfully completed", result.message);
            _mockUnitOfWorkFactory.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockUserRepo.Verify(r => r.GetUserByName(It.IsAny<string>()), Times.Once());            
            mockUnitOfWork.Verify(u => u.Save(), Times.Once());
        }

        [Test]
        public void Login_InvalidUsername_ErrorReturned()
        {
            var loginModel = new LoginViewModel { Name = "Pascoall", Password = "Ukuleke" };
            var mockUserRepo = new Mock<IUserRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.Users).Returns(mockUserRepo.Object);
            var _mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            _mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(mockUnitOfWork.Object);
            var mockSettings = new Mock<IAppSettings>();
            mockSettings.Setup(s => s["key"]).Returns("h3a4t6e4o1rrrrrr3334444");
            var mockTokenHandler = new Mock<ITokenHandler>();
            var accountService = new AccountService(_mockUnitOfWorkFactory.Object, mockTokenHandler.Object);

            var result = accountService.Login(loginModel);

            Assert.AreEqual(false, result.success);
            Assert.AreEqual("User not found", result.message);
            Assert.AreEqual(null, result.responseContent);            
            _mockUnitOfWorkFactory.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockUserRepo.Verify(r => r.GetUserByName(It.IsAny<string>()), Times.Once());            
        }

        [Test]
        public void Login_InvalidPassword_ErrorReturned()
        {
            var loginModel = new LoginViewModel { Name = "TeamNorth", Password = "HearTheRoarR" };
            var user = new User { Id = 6, Name = "TeamNorth", Password = "i??H?????????????|v?o?I??y??", Balance = 0 };
            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(r => r.GetUserByName("TeamNorth")).Returns(user);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.Users).Returns(mockUserRepo.Object);
            var _mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            _mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(mockUnitOfWork.Object);
            var mockSettings = new Mock<IAppSettings>();
            mockSettings.Setup(s => s["key"]).Returns("h3a4t6e4o1rrrrrr3334444");
            var mockTokenHandler = new Mock<ITokenHandler>();
            var accountService = new AccountService(_mockUnitOfWorkFactory.Object, mockTokenHandler.Object);

            var result = accountService.Login(loginModel);

            Assert.AreEqual(false, result.success);
            Assert.AreEqual("Password is wrong", result.message);
            Assert.AreEqual(null, result.responseContent);
            _mockUnitOfWorkFactory.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockUserRepo.Verify(r => r.GetUserByName(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Login_ValidUserData_LoginSuccessful()
        {
            var loginModel = new LoginViewModel { Name = "TeamNorth", Password = "HearTheRoar" };
            var user = new User { Id = 6, Name = "TeamNorth", Password = "i??H?????????????|v?o?I??y??", Balance = 0 };
            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(r => r.GetUserByName("TeamNorth")).Returns(user);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.Users).Returns(mockUserRepo.Object);
            var _mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            _mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(mockUnitOfWork.Object);
            var mockSettings = new Mock<IAppSettings>();
            mockSettings.Setup(s => s["key"]).Returns("h3a4t6e4o1rrrrrr3334444");
            var mockTokenHandler = new Mock<ITokenHandler>();
            var accountService = new AccountService(_mockUnitOfWorkFactory.Object, mockTokenHandler.Object);

            var result = accountService.Login(loginModel);

            Assert.AreEqual(true, result.success);
            Assert.AreEqual("Login successful", result.message);
            _mockUnitOfWorkFactory.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockUserRepo.Verify(r => r.GetUserByName(It.IsAny<string>()), Times.Once());
        }
    }
}

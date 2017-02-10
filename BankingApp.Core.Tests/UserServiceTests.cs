using BankingApp.Core.UserServices;
using BankingApp.DataRepository.DataContext;
using BankingApp.DataRepository.Repositories;
using BankingApp.DataRepository.UnitOfWork;
using BankingApp.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BankingApp.Core.Tests
{
    public class UserServiceTests
    {
        [Test]
        public void GetRegisteredUsers_WrongIdToExclude_ErrorMessage()
        {
            var _mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(mockUnitOfWork.Object);
            var userService = new UserService(_mockUnitOfWorkFactory.Object);
            Random random = new Random();
            
            int randomWrongId = random.Next(-100, 0);
            var result = userService.GetRegisteredUsers(randomWrongId);

            Assert.AreEqual(false,result.success);
            Assert.AreEqual("Wrong id", result.message);
            Assert.AreEqual(null, result.responseContent);
            _mockUnitOfWorkFactory.Verify(mock => mock.GetUnitOfWork(), Times.Never());
        }

        [Test]
        public void GetRegisteredUsers_ValidIdToExclude_UsersReturned()
        {
            var users = new List<User> { new User{Id=1,Name="Vito",Balance=25000},
                                         new User{Id=2,Name="Dupreeh",Balance=1100},
                                         new User{Id=3,Name="Magisk",Balance=0},
                                         new User{Id=4,Name="James",Balance=25}
                                       };                      
            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(r => r.GetAll()).Returns(users);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.Users).Returns(mockUserRepo.Object);
            var _mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            _mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork()).Returns(mockUnitOfWork.Object);
            var userService = new UserService(_mockUnitOfWorkFactory.Object);
            int idToExclude = 2;

            var result = userService.GetRegisteredUsers(idToExclude);

            Assert.AreEqual(true, result.success);
            Assert.AreNotEqual(null, result.responseContent);
            Assert.AreEqual(3, result.responseContent.Count);
            foreach (var user in result.responseContent)
            {
                Assert.AreNotEqual(idToExclude, user.Id);
            }
            _mockUnitOfWorkFactory.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockUserRepo.Verify(r => r.GetAll(), Times.Once());
        }
    }
}

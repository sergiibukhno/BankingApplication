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
            var _mock = new Mock<IUnitOfWorkFactory>();
            var mockUnit = new Mock<IUnitOfWork>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var userService = new UserService(_mock.Object);
            Random random = new Random();
            
            int randomWrongId = random.Next(-100, 0);
            var result = userService.GetRegisteredUsers(randomWrongId);

            Assert.AreEqual(false,result.success);
            Assert.AreEqual("Wrong id", result.message);
            Assert.AreEqual(null, result.responseContent);
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Never());
        }

        [Test]
        public void GetRegisteredUsers_ValidIdToExclude_UsersReturned()
        {
            var users = new List<User> { new User{Id=1,Name="Vito",Balance=25000},
                                         new User{Id=2,Name="Dupreeh",Balance=1100},
                                         new User{Id=3,Name="Magisk",Balance=0},
                                         new User{Id=4,Name="James",Balance=25}
                                       };                      
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetAll()).Returns(users);
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var userService = new UserService(_mock.Object);
            int idToExclude = 2;

            var result = userService.GetRegisteredUsers(idToExclude);

            Assert.AreEqual(true, result.success);
            Assert.AreNotEqual(null, result.responseContent);
            Assert.AreEqual(3, result.responseContent.Count);
            foreach (var user in result.responseContent)
            {
                Assert.AreNotEqual(idToExclude, user.Id);
            }
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockRepo.Verify(r => r.GetAll(), Times.Once());
        }
    }
}

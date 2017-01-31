using BankingApp.Core.FinancialServices;
using BankingApp.DataRepository.Repositories;
using BankingApp.DataRepository.UnitOfWork;
using BankingApp.Models;
using BankingApp.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Core.Tests
{
    public class FinancialServiceTests
    {
        [Test]
        public void GetBalance_InvalidUserId_ErrorReturned()
        {            
            var mockRepo = new Mock<IUserRepository>();            
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);            
            var financialService = new FinancialService(_mock.Object);

            var result = financialService.GetBalance(-2);

            Assert.AreEqual(false, result.success);
            Assert.AreEqual("User not found", result.message);            
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockRepo.Verify(r => r.GetUser(It.IsAny<int>()), Times.Once());            
        }

        [Test]
        public void GetBalance_ValidUserId_BalanceReturned()
        {
            var user = new User { Id = 7, Name = "Anna", Balance = 2700 };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetUser(7)).Returns(user);
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var financialService = new FinancialService(_mock.Object);

            var result = financialService.GetBalance(7);

            Assert.AreEqual(true, result.success);
            Assert.AreEqual(2700, result.responseContent);
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockRepo.Verify(r => r.GetUser(It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void GetTransactionsStatements_ValidUserId_AllTransactionsReturned()
        {
            var user = new User { Id = 7, Name = "Anna", Balance = 2700 };
            var transactions = new List<FinancialTransaction> { new FinancialTransaction{Amount=100,TransactionTime=new DateTime(2008, 3, 1, 7, 0, 0)},
                                         new FinancialTransaction{Amount=-100,TransactionTime=new DateTime(2008, 4, 22, 7, 0, 0)},
                                         new FinancialTransaction{Amount=700,TransactionTime=new DateTime(2008, 5, 2, 7, 0, 0)},
                                         new FinancialTransaction{Amount=-200,TransactionTime=new DateTime(2008, 6, 27, 7, 0, 0)}
                                       };                   
            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(r => r.GetUser(7)).Returns(user);
            var mockTransactionRepo = new Mock<IFinancialTransactionRepository>();
            mockTransactionRepo.Setup(t => t.Get(7)).Returns(transactions);
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockUserRepo.Object);
            mockUnit.Setup(u => u.Transactions).Returns(mockTransactionRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var financialService = new FinancialService(_mock.Object);

            var result = financialService.GetTransactionsStatements(7);

            Assert.AreEqual(true, result.success);
            Assert.AreEqual(4, result.responseContent.Count);
            Assert.AreEqual(100, result.responseContent[0].amount);
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockUserRepo.Verify(r => r.GetUser(It.IsAny<int>()), Times.Once());
            mockTransactionRepo.Verify(t=>t.Get(It.IsAny<int>()),Times.Once());
        }

        [Test]
        public void GetTransactionsStatements_InvalidUserId_ErrorReturned()
        {            
            var mockUserRepo = new Mock<IUserRepository>();            
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockUserRepo.Object);            
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var financialService = new FinancialService(_mock.Object);

            var result = financialService.GetTransactionsStatements(-1);

            Assert.AreEqual(false, result.success);
            Assert.AreEqual(null, result.responseContent);
            Assert.AreEqual("User not found", result.message);
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockUserRepo.Verify(r => r.GetUser(It.IsAny<int>()), Times.Once());            
        }

        [Test]
        public void PerformFinancialOperation_TransferMoney_ResultSuccess()
        {
            var sendingUser = new User { Id = 7, Name = "Anna", Balance = 2700 };
            var receivingUser = new User { Id = 5, Name = "Olya", Balance = 200 };
            var transferModel = new TransferViewModel { userId = 7, toUserId = 5, amount = 700 };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetUser(7)).Returns(sendingUser);
            mockRepo.Setup(r => r.GetUser(5)).Returns(receivingUser);
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var financialService = new FinancialService(_mock.Object);

            var result = financialService.PerformFinancialOperation(transferModel);

            Assert.AreEqual(true, result.success);
            Assert.AreEqual(2000, result.responseContent);
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockRepo.Verify(r => r.GetUser(It.IsAny<int>()), Times.Exactly(2));
            mockUnit.Verify(u => u.Save(), Times.Once());
        }

        [Test]
        public void PerformFinancialOperation_DepositMoney_ResultSuccess()
        {
            var user = new User { Id = 7, Name = "Anna", Balance = 2700 };            
            var depositModel = new DepositViewModel { userId = 7, amount = 300 };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetUser(7)).Returns(user);            
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var financialService = new FinancialService(_mock.Object);

            var result = financialService.PerformFinancialOperation(depositModel);

            Assert.AreEqual(true, result.success);
            Assert.AreEqual(3000, result.responseContent);
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockRepo.Verify(r => r.GetUser(It.IsAny<int>()), Times.Once());
            mockUnit.Verify(u => u.Save(), Times.Once());
        }

        [Test]
        public void PerformFinancialOperation_WithdrawMoney_ResultSuccess()
        {
            var user = new User { Id = 7, Name = "Anna", Balance = 2700 };
            var withdrawModel = new WithdrawViewModel { userId = 7, amount = 2700 };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetUser(7)).Returns(user);
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var financialService = new FinancialService(_mock.Object);

            var result = financialService.PerformFinancialOperation(withdrawModel);

            Assert.AreEqual(true, result.success);
            Assert.AreEqual(0, result.responseContent);
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockRepo.Verify(r => r.GetUser(It.IsAny<int>()), Times.Once());
            mockUnit.Verify(u => u.Save(), Times.Once());
        }

        [Test]
        public void PerformFinancialOperation_NotEnoughMoney_ErrorReturned()
        {
            var user = new User { Id = 7, Name = "Anna", Balance = 2700 };
            var withdrawModel = new WithdrawViewModel { userId = 7, amount = 3000 };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetUser(7)).Returns(user);
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var financialService = new FinancialService(_mock.Object);

            var result = financialService.PerformFinancialOperation(withdrawModel);

            Assert.AreEqual(false, result.success);            
            Assert.AreEqual("You dont have enough money", result.message);
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockRepo.Verify(r => r.GetUser(It.IsAny<int>()), Times.Once());
            mockUnit.Verify(u => u.Save(), Times.Never());
        }

        [Test]
        public void PerformFinancialOperation_WrongUserId_ErrorReturned()
        {            
            var withdrawModel = new WithdrawViewModel { userId = -2, amount = 3000 };
            var mockRepo = new Mock<IUserRepository>();            
            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.Setup(u => u.Users).Returns(mockRepo.Object);
            var _mock = new Mock<IUnitOfWorkFactory>();
            _mock.Setup(m => m.GetUnitOfWork()).Returns(mockUnit.Object);
            var financialService = new FinancialService(_mock.Object);

            var result = financialService.PerformFinancialOperation(withdrawModel);

            Assert.AreEqual(false, result.success);
            Assert.AreEqual("User not found", result.message);
            _mock.Verify(mock => mock.GetUnitOfWork(), Times.Once());
            mockRepo.Verify(r => r.GetUser(It.IsAny<int>()), Times.Once());
            mockUnit.Verify(u => u.Save(), Times.Never());
        }
    }
}

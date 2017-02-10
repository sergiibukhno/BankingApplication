using BankingApp.DataRepository.UnitOfWork;
using BankingApp.Models;
using BankingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;

namespace BankingApp.Core.FinancialServices
{
    public class FinancialService:IFinancialService
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private static readonly object locker = new object();

        public FinancialService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public ResponseViewModel<double> GetBalance(int userId)
        {
            using(var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                var user = unitOfWork.Users.GetUser(userId);

                if (user != null)
                {
                    return new ResponseViewModel<double> { responseContent=user.Balance,success=true};
                }
                
                else
                {
                    return new ResponseViewModel<double> { message="User not found",success=false};
                }
            }            
        }
        
        public ResponseViewModel<List<TransactionViewModel>> GetTransactionsStatements(int userId)
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                var user = unitOfWork.Users.GetUser(userId);
                if (user != null)
                {
                    var userTransactions = unitOfWork.Transactions.Get(userId)
                        .Select(r => new TransactionViewModel
                             {
                                 amount = r.Amount,
                                 TransactionTime = r.TransactionTime

                             }).ToList();

                    return new ResponseViewModel<List<TransactionViewModel>> { responseContent = userTransactions, success = true };
                }

                else
                {
                    return new ResponseViewModel<List<TransactionViewModel>> { message = "User not found", success = false };
                }                
            }
        }
        
        public ResponseViewModel<double> Deposit(DepositViewModel depositModel)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
                    {
                        var user = unitOfWork.Users.GetUser(depositModel.userId);
                        if (user != null)
                        {
                            user.Balance += depositModel.amount;
                            AddTransaction(user, depositModel.amount);
                            unitOfWork.Save();
                            transactionScope.Complete();
                            return new ResponseViewModel<double> { responseContent = user.Balance, success = true };
                        }
                        else
                        {
                            return new ResponseViewModel<double> { message = "User not found", success = false };
                        }
                    }
                }
                catch (DbUpdateException)
                {
                    var result = Deposit(depositModel);
                    return result;
                }
            }
        }

        public ResponseViewModel<double> Withdraw(WithdrawViewModel withdrawModel)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
                    {
                        var user = unitOfWork.Users.GetUser(withdrawModel.userId);
                        if (user != null)
                        {
                            if (user.Balance >= withdrawModel.amount)
                            {
                                user.Balance -= withdrawModel.amount;
                                AddTransaction(user, -withdrawModel.amount);
                                unitOfWork.Save();
                                transactionScope.Complete();
                                return new ResponseViewModel<double> { responseContent = user.Balance, success = true };
                            }
                            else
                            {
                                return new ResponseViewModel<double> { message = "You dont have enough money", success = false };
                            }
                        }
                        else
                        {
                            return new ResponseViewModel<double> { message = "User not found", success = false };
                        }
                    }
                }
                catch (DbUpdateException)
                {
                    var result = Withdraw(withdrawModel);
                    return result;
                }                
            }
        }

        public ResponseViewModel<double> Transfer(TransferViewModel transferModel)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
                    {
                        var sendingUser = unitOfWork.Users.GetUser(transferModel.userId);
                        var receivingUser = unitOfWork.Users.GetUser(transferModel.toUserId);
                        if (sendingUser != null && receivingUser != null)
                        {
                            if (sendingUser.Balance >= transferModel.amount)
                            {
                                sendingUser.Balance -= transferModel.amount;
                                receivingUser.Balance += transferModel.amount;
                                AddTransaction(sendingUser, -transferModel.amount);
                                AddTransaction(receivingUser, transferModel.amount);
                                unitOfWork.Save();
                                transactionScope.Complete();
                                return new ResponseViewModel<double> { responseContent = sendingUser.Balance, success = true };
                            }
                            else
                            {
                                return new ResponseViewModel<double> { message = "You dont have enough money", success = false };
                            }
                        }
                        else
                        {
                            return new ResponseViewModel<double> { message = "User not found", success = false };
                        }
                    }
                }
                catch (DbUpdateException)
                {
                    var result = Transfer(transferModel);
                    return result;
                }                
            }
        }
        
        public void AddTransaction(User user, double amount)
        {
            if (user.FinancialTransactions == null)
                user.FinancialTransactions = new Collection<FinancialTransaction>();

            user.FinancialTransactions.Add(new FinancialTransaction() { Amount = amount,TransactionTime=DateTime.Now });
        }
    }
}

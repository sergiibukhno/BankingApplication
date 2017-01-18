using BankingApp.DataRepository.UnitOfWork;
using BankingApp.Models;
using BankingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Linq;

namespace BankingApp.Core.FinancialServices
{
    public class FinancialService:IFinancialService
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;

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
                    return new ResponseViewModel<double> { errorMessage="User not found",success=false};
                }
            }            
        }

        public ResponseViewModel<double> Deposite(int userId, double amount)
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                var user = unitOfWork.Users.GetUser(userId);

                if (user != null)
                {
                    user.Balance += amount;
                    AddTransaction(user, amount);
                    unitOfWork.Save();
                    return new ResponseViewModel<double> { responseContent = user.Balance, success = true };
                }

                else
                {
                    return new ResponseViewModel<double> { errorMessage = "User not found", success = false };
                }                
            }
        }

        public ResponseViewModel<double> Withdraw(int userId, double amount)
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                var user = unitOfWork.Users.GetUser(userId);

                if (user != null)
                {
                    if (user.Balance >= amount)
                    {
                        user.Balance -= amount;
                        AddTransaction(user, -amount);
                        unitOfWork.Save();
                        return new ResponseViewModel<double> { responseContent = user.Balance, success = true };
                    }
                    else
                    {
                        return new ResponseViewModel<double> { errorMessage = "You dont have enough money", success = false };
                    }
                }
                
                else
                {
                    return new ResponseViewModel<double> { errorMessage = "User not found", success = false };
                }                
            }
        }

        public ResponseViewModel<double> Transfer(TransferViewModel transferModel)
        {
            if (transferModel.toUserId <= 0 || transferModel.amount <= 0)
            {
                return new ResponseViewModel<double> { errorMessage = "Check input data", success = false };
            }

            if (transferModel.fromUserId == transferModel.toUserId)
            {
                return new ResponseViewModel<double> { errorMessage = "Cannot transfer to yourself", success = false };
            }

            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                var sendingUser = unitOfWork.Users.GetUser(transferModel.fromUserId);
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
                        return new ResponseViewModel<double> { responseContent = sendingUser.Balance, success = true };
                    }
                    else
                    {
                        return new ResponseViewModel<double> { errorMessage = "You dont have enough money", success = false };
                    }
                }
                
                else
                {
                    return new ResponseViewModel<double> { errorMessage = "User not found", success = false };
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
                    return new ResponseViewModel<List<TransactionViewModel>> { errorMessage = "User not found", success = false };
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

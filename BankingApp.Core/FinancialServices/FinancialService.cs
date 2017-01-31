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

        public ResponseViewModel<double> PerformFinancialOperation(FinancialOperationViewModel financialOperation)
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                if (financialOperation.GetType() == typeof(TransferViewModel))
                {                                        
                        var sendingUser = unitOfWork.Users.GetUser(financialOperation.userId);
                        var receivingUser = unitOfWork.Users.GetUser(financialOperation.toUserId);

                        if (sendingUser != null && receivingUser != null)
                        {
                            if (sendingUser.Balance >= financialOperation.amount)
                            {
                                sendingUser.Balance -= financialOperation.amount;
                                receivingUser.Balance += financialOperation.amount;

                                AddTransaction(sendingUser, -financialOperation.amount);
                                AddTransaction(receivingUser, financialOperation.amount);
                                unitOfWork.Save();
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

                else
                {
                    var user = unitOfWork.Users.GetUser(financialOperation.userId);

                    if (user != null)
                    {
                        if (financialOperation.GetType() == typeof(DepositViewModel))
                        {
                            user.Balance += financialOperation.amount;
                            AddTransaction(user, financialOperation.amount);
                            unitOfWork.Save();
                            return new ResponseViewModel<double> { responseContent = user.Balance, success = true };
                        }

                        else
                            if (financialOperation.GetType() == typeof(WithdrawViewModel))
                            {
                                if (user.Balance >= financialOperation.amount)
                                {
                                    user.Balance -= financialOperation.amount;
                                    AddTransaction(user, -financialOperation.amount);
                                    unitOfWork.Save();
                                    return new ResponseViewModel<double> { responseContent = user.Balance, success = true };
                                }
                                else
                                {
                                    return new ResponseViewModel<double> { message = "You dont have enough money", success = false };
                                }
                            }
                            else
                            {
                                return new ResponseViewModel<double> { message = "Contact administrator", success = false };
                            }
                    }
                    else
                    {
                        return new ResponseViewModel<double> { message = "User not found", success = false };
                    }
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

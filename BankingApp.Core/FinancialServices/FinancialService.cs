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
        private IUnitOfWork _unitOfWork;

        public FinancialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public HttpResponseMessage GetBalance(int userId)
        {
            try
            {
                var user = _unitOfWork.Users.Get(userId);

                if (user != null)
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response.Content = new ObjectContent(user[0].Balance.GetType(),user[0].Balance,new XmlMediaTypeFormatter());
                    return response;
                }
                
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest); 
                }
                
            }
            
            catch (Exception ex)
            {
                
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        public HttpResponseMessage Deposite(int userId, double amount)
        {
            try
            {
                var user = _unitOfWork.Users.Get(userId);

                if (user != null)
                {
                    user[0].Balance += amount;
                    AddTransaction(user[0], amount);
                    _unitOfWork.Save();
                }
                
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }                

                return GetBalance(userId);
            }
            
            catch (Exception ex)
            {
                
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        public HttpResponseMessage Withdraw(int userId, double amount)
        {
            try
            {
                    var user = _unitOfWork.Users.Get(userId);

                    if (user != null)
                    {
                        if (user[0].Balance >= amount)
                        {
                            user[0].Balance -= amount;
                            AddTransaction(user[0], -amount);
                            _unitOfWork.Save();
                        }
                        else
                        {
                            return new HttpResponseMessage(HttpStatusCode.BadRequest);
                        }
                    }
                    else
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest);
                    }
                
                return GetBalance(userId);
            }
            
            catch (Exception ex)
            {
                
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        public HttpResponseMessage Transfer(int fromUserId, int toUserId, double amount)
        {
            if (toUserId <= 0 || amount <= 0)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            if (fromUserId == toUserId)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            
            try
            {
                    var sendingUser = _unitOfWork.Users.Get(fromUserId);
                    var receivingUser = _unitOfWork.Users.Get(toUserId);

                    if (sendingUser != null && receivingUser != null)
                    {
                        if (sendingUser[0].Balance >= amount)
                        {
                            sendingUser[0].Balance -= amount;
                            receivingUser[0].Balance += amount;

                            AddTransaction(sendingUser[0], -amount);
                            AddTransaction(receivingUser[0], amount);
                            _unitOfWork.Save();
                        }
                        else
                        {
                            return new HttpResponseMessage(HttpStatusCode.BadRequest);
                        }
                    }
                    else
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest);
                    }
                
                return GetBalance(fromUserId);
            }
            
            catch (Exception ex)
            {
               
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        public HttpResponseMessage GetTransactionsStatements(int userId)
        {
            try
            {
                var userTransactions = _unitOfWork.Transactions.Get(userId)
                    .Select(r => new Transaction
                         {
                             amount = r.Amount,
                             TransactionTime = r.TransactionTime

                         }).ToList();
                
                HttpResponseMessage response = new HttpResponseMessage();
                response.Content = new ObjectContent(userTransactions.GetType(), userTransactions, new XmlMediaTypeFormatter());
                return response;                                
            }
            
            catch (Exception ex)
            {
                
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        
        public void AddTransaction(Models.User user, double amount)
        {
            if (user.FinancialTransactions == null)
                user.FinancialTransactions = new Collection<FinancialTransaction>();

            user.FinancialTransactions.Add(new FinancialTransaction() { Amount = amount,TransactionTime=DateTime.Now });
        }
    }
}

using System.Text;
using System.Security.Cryptography;
using JWT;
using System.Collections.Generic;
using System;
using BankingApp.Models;
using BankingApp.ViewModels;
using BankingApp.DataRepository.UnitOfWork;
using System.Configuration;
using System.Collections.Specialized;

namespace BankingApp.Core.AccountServices
{
    public class AccountService:IAccountService
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private IAppSettings _appSettings;

        public AccountService(IUnitOfWorkFactory unitOfWorkFactory,IAppSettings appSettings)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _appSettings = appSettings;
        }
        
        public string EncryptPassword(string inputString)
        {
            byte[] data = Encoding.ASCII.GetBytes(inputString);
            data = new SHA256Managed().ComputeHash(data);
            string hash = Encoding.ASCII.GetString(data);
            return hash;
        }

        public string CreateToken(User user)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddDays(30) - unixEpoch).TotalSeconds);
          
            var payload = new Dictionary<string, object>
            {
                {"name", user.Name},
                {"userId", user.Id},
                {"exp", expiry}
            };

            string key = _appSettings["key"];

            var token = JsonWebToken.Encode(payload, key, JwtHashAlgorithm.HS256);
            
            return token;
        }

        public ResponseViewModel<string> Register(RegisterViewModel registerModel)
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                var existingUser = unitOfWork.Users.GetUserByName(registerModel.Name);

                if (existingUser != null)
                {
                    return new ResponseViewModel<string> { message = "User already exists", success = false };
                }

                var user = new User
                {
                    Name = registerModel.Name,
                    Password = EncryptPassword(registerModel.Password),
                    Balance = 0
                };

                unitOfWork.Users.Create(user);
                unitOfWork.Save();                
                var token = CreateToken(user);

                return new ResponseViewModel<string> { responseContent = token, message = "Registration successfully completed", success = true };
            }           
        }

        public ResponseViewModel<string> Login(LoginViewModel loginModel)
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                var existingUser = unitOfWork.Users.GetUserByName(loginModel.Name);

                if (existingUser == null)
                {
                    return new ResponseViewModel<string> { message = "User not found", success = false };
                }
                else
                {
                    var loginSuccess =
                        string.Equals(EncryptPassword(loginModel.Password),
                            existingUser.Password);

                    if (loginSuccess)
                    {
                        var token = CreateToken(existingUser);
                        return new ResponseViewModel<string> { responseContent = token, message = "Login successful", success = true };
                    }
                    else
                    {
                        return new ResponseViewModel<string> { message = "Password is wrong", success = false };
                    }
                }                
            }
        }
    }
    
    public interface IAppSettings
    {
        string this[string key] { get; }
    }

    public class AppSettingsWrapper : IAppSettings
    {
        private readonly NameValueCollection appSettings;

        public AppSettingsWrapper()
        {
            this.appSettings = ConfigurationManager.AppSettings;
        }
        public string this[string key]
        {
            get
            {
                return this.appSettings[key];
            }
        }
    }
}

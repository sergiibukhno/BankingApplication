using System.Text;
using System.Security.Cryptography;
using BankingApp.Models;
using BankingApp.ViewModels;
using BankingApp.DataRepository.UnitOfWork;
using BankingApp.Core.TokenHandler;

namespace BankingApp.Core.AccountServices
{
    public class AccountService:IAccountService
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private ITokenHandler _tokenHandler;

        public AccountService(IUnitOfWorkFactory unitOfWorkFactory,ITokenHandler tokenHandler)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _tokenHandler = tokenHandler;
        }
        
        public string EncryptPassword(string inputString)
        {
            byte[] data = Encoding.ASCII.GetBytes(inputString);
            data = new SHA256Managed().ComputeHash(data);
            string hash = Encoding.ASCII.GetString(data);
            return hash;
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
                var token = _tokenHandler.CreateToken(user);

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
                        var token = _tokenHandler.CreateToken(existingUser);
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
}

using BankingApp.Models;
using System.Security.Claims;

namespace BankingApp.Core.TokenHandler
{
    public interface ITokenHandler
    {
        string CreateToken(User user);
        ClaimsPrincipal ValidateToken(string token, string secretKey);
    }
}

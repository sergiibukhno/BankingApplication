using BankingApp.Core.AppSettings;
using BankingApp.Models;
using JWT;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Script.Serialization;

namespace BankingApp.Core.TokenHandler
{
    public class TokenHandler:ITokenHandler
    {
        private IAppSettings _appSettings;
        private const string nameClaimType = "name";
        private const string idClaimType = "userId";
        private const string expClaimType = "exp";
        private const string identityType = "Federation";
        private DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public TokenHandler(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public string CreateToken(User user)
        {
            var expiry = Math.Round((DateTime.UtcNow.AddDays(30) - unixEpoch).TotalSeconds);

            var payload = new Dictionary<string, object>
            {
                {nameClaimType, user.Name},
                {idClaimType, user.Id},
                {expClaimType, expiry}
            };

            string key = _appSettings["key"];

            var token = JsonWebToken.Encode(payload, key, JwtHashAlgorithm.HS256);

            return token;
        }

        public ClaimsPrincipal ValidateToken(string token, string secretKey)
        {
            var jsonSerializer = new JavaScriptSerializer();
            var payloadJson = JsonWebToken.Decode(token, secretKey);
            var payloadData = jsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);

            object exp;

            if (payloadData != null && payloadData.TryGetValue(expClaimType, out exp))
            {
                var validTo = unixEpoch.AddSeconds(long.Parse(exp.ToString()));
                if (DateTime.Compare(validTo, DateTime.UtcNow) <= 0)
                {
                    throw new Exception("Token is expired");
                }
            }

            var claimsIdentity = new ClaimsIdentity(identityType);
            var claims = new List<Claim>();

            if (payloadData != null)
                foreach (var pair in payloadData)
                {
                    switch (pair.Key)
                    {
                        case nameClaimType:
                            claims.Add(new Claim(ClaimTypes.Name, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                        case idClaimType:
                            claims.Add(new Claim(ClaimTypes.UserData, pair.Value.ToString(), ClaimValueTypes.Integer));
                            break;
                        default:
                            claims.Add(new Claim(pair.Key, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                    }
                }

            claimsIdentity.AddClaims(claims);
            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}

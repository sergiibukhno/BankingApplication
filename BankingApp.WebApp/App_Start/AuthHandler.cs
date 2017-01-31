using JWT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace BankingApp.WebApp.App_Start
{
    public class AuthHandler:DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage errorResponse = null;

            try
            {
                IEnumerable<string> requestHeaderValues;
                request.Headers.TryGetValues("Authorization", out requestHeaderValues);

                if (requestHeaderValues == null)
                    return base.SendAsync(request, cancellationToken);

                var bearerToken = requestHeaderValues.ElementAt(0);
                var token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
                var key = WebConfigurationManager.AppSettings["key"];

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = ValidateToken(token, key);
                }
            }
            catch (Exception exception)
            {
                errorResponse = request.CreateErrorResponse(HttpStatusCode.BadRequest, exception.Message);
            }

            return errorResponse != null ? Task.FromResult(errorResponse) : base.SendAsync(request, cancellationToken);
        }

        private ClaimsPrincipal ValidateToken(string token, string secretKey)
        {
            var jsonSerializer = new JavaScriptSerializer();
            var payloadJson = JsonWebToken.Decode(token, secretKey);
            var payloadData = jsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);

            object exp;

            if (payloadData != null && payloadData.TryGetValue("exp", out exp))
            {
                var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var validTo = unixEpoch.AddSeconds(long.Parse(exp.ToString()));
                if (DateTime.Compare(validTo, DateTime.UtcNow) <= 0)
                {
                    throw new Exception("Token is expired");
                }
            }

            var claimsIdentity = new ClaimsIdentity("Federation");
            var claims = new List<Claim>();

            if (payloadData != null)
                foreach (var pair in payloadData)
                {
                    switch (pair.Key)
                    {
                        case "name":
                            claims.Add(new Claim(ClaimTypes.Name, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                        case "userId":
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
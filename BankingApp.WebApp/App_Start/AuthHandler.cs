using BankingApp.Core.AppSettings;
using BankingApp.Core.TokenHandler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace BankingApp.WebApp.App_Start
{
    public class AuthHandler:DelegatingHandler
    {
        private ITokenHandler _tokenHandler;
        private IAppSettings _appSettings;
        private const string authorizationHeaderValue = "Authorization";
        private const string authorizationBearerValue = "Bearer ";

        public AuthHandler()
        {
            _appSettings=new AppSettingsWrapper();
            _tokenHandler = new TokenHandler(_appSettings);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage errorResponse = null;

            try
            {
                IEnumerable<string> requestHeaderValues;
                request.Headers.TryGetValues(authorizationHeaderValue, out requestHeaderValues);

                if (requestHeaderValues == null)
                    return base.SendAsync(request, cancellationToken);

                var bearerToken = requestHeaderValues.ElementAt(0);
                var token = bearerToken.StartsWith(authorizationBearerValue) ? bearerToken.Substring(7) : bearerToken;
                var key = WebConfigurationManager.AppSettings["key"];

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = _tokenHandler.ValidateToken(token, key);
                }
            }
            catch (Exception exception)
            {
                errorResponse = request.CreateErrorResponse(HttpStatusCode.BadRequest, exception.Message);
            }

            return errorResponse != null ? Task.FromResult(errorResponse) : base.SendAsync(request, cancellationToken);
        }
    }
}
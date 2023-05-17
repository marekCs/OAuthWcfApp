using Newtonsoft.Json;
using OAuthWcfApp.Authorize;
using OAuthWcfApp.Models;
using System;

namespace OAuthWcfApp.Services
{
    public class TokenService : ITokenService
    {
        private readonly LoginValidator _loginValidator;
        private readonly TokenHandlerService _tokenHandlerService;
        public TokenService()
        {
            _loginValidator = new LoginValidator();
            _tokenHandlerService = new TokenHandlerService();
        }
        public string Authorize(string loginPasswordInJsonFormat)
        {
            LoginPasswordData data = JsonConvert.DeserializeObject<LoginPasswordData>(loginPasswordInJsonFormat);
            if (_loginValidator.ValidateCredentials(data.Login, data.Password))
            {
                // Create a new GUID as an authorization grant.
                string authorizationGrant = Guid.NewGuid().ToString();

                // We store the authorization grant and login information in our dictionary.
                UserModel user = new UserModel
                {
                    Id = 1,
                    Login = "test",
                    Role = UserRoles.Admin
                };
                _tokenHandlerService.StoreAuthorizationGrant(authorizationGrant, user);

                // This is essentially a temporary code that the client receives as confirmation that the user has agreed to grant access.
                return JsonConvert.SerializeObject(new { success = true, token = authorizationGrant });
            }
            else
            {
                // return "Invalid login or password";
                return JsonConvert.SerializeObject(new { success = false, error = "Invalid login or password" });
            }
        }

        public string Exchange(string authorizationGrant)
        {
            return _tokenHandlerService.Exchange(authorizationGrant);
        }
    }
}

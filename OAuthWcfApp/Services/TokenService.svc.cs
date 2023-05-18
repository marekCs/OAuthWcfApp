using Newtonsoft.Json;
using OAuthWcfApp.Authorize;
using OAuthWcfApp.Models;
using System;
using System.IO;

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
        public string Authorize(string login, string password)
        {
            if (_loginValidator.ValidateCredentials(login, password))
            {
                // Create a new GUID as an authorization grant.
                string authorizationGrant = Guid.NewGuid().ToString();

                // We store the authorization grant and login information in our dictionary.
                Random random = new Random();
                int randomNumber = random.Next(5, 1001);
                UserModel user = new UserModel
                {
                    Id = randomNumber,
                    Login = "test",
                    Role = UserRoles.Admin
                };
                _tokenHandlerService.StoreAuthorizationGrant(authorizationGrant, user);

                // This is essentially a temporary code that the client receives as confirmation that the user has agreed to grant access.
                return JsonConvert.SerializeObject(new { Success = true, Token = authorizationGrant, Message = "Successful authorization!" });
            }
            else
            {
                // return "Invalid login or password";
                return JsonConvert.SerializeObject(new { Success = false, Token = "", Message = "Invalid login or password!" });
            }
        }

        public string Exchange(string authorizationGrant)
        {
            // První kontrola zda existuje daný grant token.
            var storedUser = _tokenHandlerService.GetAssociatedUserWithGrant(authorizationGrant);

            if (storedUser == null)
            {
                // Pokud ne, vrátit chybovou zprávu.
                return JsonConvert.SerializeObject(new { JwtToken = "", Success = false, Message = "Invalid authorization grant!" });
            }

            // Pokud existuje, pokračovat s generováním přístupového tokenu.
            string token = _tokenHandlerService.Exchange(authorizationGrant);

            if (string.IsNullOrEmpty(token))
            {
                return JsonConvert.SerializeObject(new { JwtToken = "", Success = false });
            }
            else
            {
                return JsonConvert.SerializeObject(new { JwtToken = token, Success = true });
            }
        }

    }
}

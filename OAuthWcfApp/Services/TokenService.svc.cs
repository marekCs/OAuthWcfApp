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
        public string Authorize(string login, string password)
        {
            if (_loginValidator.ValidateCredentials(login, password))
            {
                // Create a new GUID as an authorization grant.
                string authorizationGrant = Guid.NewGuid().ToString();

                // We store the permissions granted and login credentials in our file system (for demonstration purposes only), on the live server we store this in the database.
                // Similarly we generate users randomly from de in code, but here you pull the user data from the database as well
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
            // First check if the grant token exists.
            var storedUser = _tokenHandlerService.GetAssociatedUserWithGrant(authorizationGrant);

            if (storedUser == null)
            {
                // If not, return an error message.
                return JsonConvert.SerializeObject(new { JwtToken = "", Success = false, Message = "Invalid authorization grant!" });
            }

            // If it exists, continue with access token generation.
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

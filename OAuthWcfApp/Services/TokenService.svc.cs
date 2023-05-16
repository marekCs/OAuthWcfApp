using OAuthWcfApp.Authorize;
using OAuthWcfApp.Models;
using System;

namespace OAuthWcfApp.Services
{
    public class TokenService : ITokenService
    {
        private LoginValidator _loginValidator;
        public TokenService()
        {
            _loginValidator = new LoginValidator();
        }
        public string Authorize(string login, string password)
        {
            if (_loginValidator.ValidateCredentials(login, password))
            {
                // Create a new GUID as an authorization grant.
                string authorizationGrant = Guid.NewGuid().ToString();

                // We store the authorization grant and login information in our dictionary.
                UserModel user = new UserModel
                {
                    Id = 1,
                    Login = "test",
                    Password = "password"
                };
                TokenHandlerService.AuthorizationGrants[authorizationGrant] = user;

                return authorizationGrant;
            }
            else
            {
                throw new Exception("Invalid login or password");
            }
        }

        public string Exchange(string authorizationGrant)
        {
            // We will check if there is an authorization grant.
            if (TokenHandlerService.AuthorizationGrants.ContainsKey(authorizationGrant))
            {
                // Create a new access token.
                string accessToken = Guid.NewGuid().ToString();

                // Move users from authorization grants to access tokens.
                TokenHandlerService.AccessTokens[accessToken] = TokenHandlerService.AuthorizationGrants[authorizationGrant];
                TokenHandlerService.AuthorizationGrants.Remove(authorizationGrant);

                return accessToken;
            }
            else
            {
                throw new Exception("Invalid authorization grant");
            }
        }
    }
}

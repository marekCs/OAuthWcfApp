using OAuthWcfApp.Authorize;
using OAuthWcfApp.Models;
using System;

namespace OAuthWcfApp.Services
{
    public class UserService : IUserService
    {
        private TokenHandlerService _tokenHandlerService;

        public UserService()
        {
            _tokenHandlerService = new TokenHandlerService();
        }

        public UserModel GetUser(string accessToken)
        {
            // Check if the access token is valid.
            if (_tokenHandlerService.ValidateToken(accessToken))
            {
                // Return the user that is associated with this access token.
                return TokenHandlerService.AccessTokens[accessToken];
            }
            else
            {
                throw new Exception("Invalid access token");
            }
        }
    }
}

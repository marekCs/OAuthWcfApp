using OAuthWcfApp.Models;
using System.Collections.Generic;
using System;

namespace OAuthWcfApp.Authorize
{
    public class TokenHandlerService
    {
        private readonly JwtTokenHandler _jwtTokenHandler;

        private Dictionary<string, UserModel> AuthorizationGrants { get; set; }
        private Dictionary<string, UserModel> AccessTokens { get; set; }

        public TokenHandlerService()
        {
            _jwtTokenHandler = new JwtTokenHandler();
            AuthorizationGrants = new Dictionary<string, UserModel>();
            AccessTokens = new Dictionary<string, UserModel>();
        }

        public void StoreAuthorizationGrant(string authorizationGrant, UserModel user)
        {
            AuthorizationGrants[authorizationGrant] = user;
        }

        public string Exchange(string authorizationGrant)
        {
            // Check if the authorization grant exists.
            if (AuthorizationGrants.ContainsKey(authorizationGrant))
            {
                UserModel user = AuthorizationGrants[authorizationGrant];
                string accessToken = _jwtTokenHandler.GenerateToken(user.Id.ToString(), user.Role);

                AccessTokens[accessToken] = user;
                AuthorizationGrants.Remove(authorizationGrant);

                return accessToken;
            }
            else
            {
                throw new Exception("Invalid authorization grant");
            }
        }

        public UserModel GetAssociatedUser(string accessToken)
        {
            if (AccessTokens.ContainsKey(accessToken))
            {
                return AccessTokens[accessToken];
            }
            else
            {
                throw new Exception("No user associated with this access token");
            }
        }

        public bool ValidateToken(string accessToken)
        {
            return _jwtTokenHandler.IsValidToken(accessToken) && AccessTokens.ContainsKey(accessToken);
        }
    }
}

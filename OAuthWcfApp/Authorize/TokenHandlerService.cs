using OAuthWcfApp.Models;
using System.Collections.Generic;
using System;

namespace OAuthWcfApp.Authorize
{
    public class TokenHandlerService
    {
        private JwtTokenHandler _jwtTokenHandler;

        public TokenHandlerService()
        {
            _jwtTokenHandler = new JwtTokenHandler();
        }

        public static Dictionary<string, UserModel> AuthorizationGrants = new Dictionary<string, UserModel>();
        public static Dictionary<string, UserModel> AccessTokens = new Dictionary<string, UserModel>();


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

        public bool ValidateToken(string accessToken)
        {
            return _jwtTokenHandler.IsValidToken(accessToken) && AccessTokens.ContainsKey(accessToken);
        }
    }
}
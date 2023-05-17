using OAuthWcfApp.Authorize;
using OAuthWcfApp.Models;
using System;
using System.Security.Permissions;

namespace OAuthWcfApp.Services
{
    public class UserService : IUserService
    {
        private readonly TokenHandlerService _tokenHandlerService;

        public UserService()
        {
            _tokenHandlerService = new TokenHandlerService();
        }
        [PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
        public UserModel GetAllUserInfo(string accessToken)
        {
            // Check if the access token is valid.
            if (_tokenHandlerService.ValidateToken(accessToken))
            {
                // Set access for specific role only if you dont want to use PrincipalPermissions everywhere
                //UserModel user = _tokenHandlerService.GetAssociatedUser(accessToken);
                //if (user.Role != UserRoles.Admin)
                //{
                //    throw new Exception("Unauthorized");
                //}
                // Return the user that is associated with this access token.
                return _tokenHandlerService.GetAssociatedUser(accessToken);
            }
            else
            {
                throw new Exception("Invalid access token");
            }
        }
    }
}

using Newtonsoft.Json;
using OAuthWcfApp.Authorize;
using OAuthWcfApp.Models;
using System;
using System.Security.Permissions;
using System.Threading;

namespace OAuthWcfApp.Services
{
    public class UserService : IUserService
    {
        private readonly TokenHandlerService _tokenHandlerService;

        public UserService()
        {
            _tokenHandlerService = new TokenHandlerService();
            string roleName = Thread.CurrentPrincipal.Identity.Name;
            _tokenHandlerService.SaveLog("Role name in UserService is: " + roleName);
        }
        [CustomPrincipalPermission("Admin")]
        public string GetAllUserInfo()
        {
            _tokenHandlerService.SaveLog("I have a permission as Admin");
            UserModel user = _tokenHandlerService.GetAssociatedUser();
            // if you dont want to use PrincipalPermission to every method, just use this:
            //if (user.Role != UserRoles.Admin)
            //{
            //    throw new Exception("Unauthorized");
            //}
            // Return the user that is associated with this access token.
            return JsonConvert.SerializeObject(user);
        }
    }
}

using Newtonsoft.Json;
using OAuthWcfApp.Authorize;
using OAuthWcfApp.Models;

namespace OAuthWcfApp.Services
{
    public class UserService : IUserService
    {
        private readonly TokenHandlerService _tokenHandlerService;

        public UserService()
        {
            _tokenHandlerService = new TokenHandlerService();
        }
        // Roles are setup in the enum UsersRole, but you can read it from database
        [CustomPrincipalPermission("Admin")]
        public string GetAllUserInfo()
        {
            UserModel user = _tokenHandlerService.GetAssociatedUser();
            // Return the more complex User info which can see just Admin.
            return JsonConvert.SerializeObject(user);
        }
    }
}

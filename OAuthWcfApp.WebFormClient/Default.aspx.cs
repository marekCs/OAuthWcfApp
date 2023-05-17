using System;
using Newtonsoft.Json;
using OAuthWcfApp.WebFormClient.TokenClient;
using OAuthWcfApp.WebFormClient.UserClient;

namespace OAuthWcfApp.WebFormClient
{
    public partial class Default : System.Web.UI.Page
    {
        [System.Web.Services.WebMethod]
        public static string AuthorizeUser(string login, string password)
        {
            try
            {
                TokenServiceClient tokenClient = new TokenServiceClient();

                var loginPasswordData = new { Login = login, Password = password };
                string loginPasswordJson = JsonConvert.SerializeObject(loginPasswordData);

                string accessToken = tokenClient.Authorize(loginPasswordJson);
                return JsonConvert.SerializeObject(new { success = true, token = accessToken });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { success = false, error = ex.Message });
            }
        }

        [System.Web.Services.WebMethod]
        public static string GetAllUserInfo(string accessToken)
        {
            try
            {
                UserServiceClient userClient = new UserServiceClient();
                var user = userClient.GetAllUserInfo(accessToken);

                // Serialize the UserModel object to a JSON string
                var userJson = JsonConvert.SerializeObject(user);

                return JsonConvert.SerializeObject(new { success = true, user = userJson });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { success = false, error = ex.Message });
            }
        }
    }
}

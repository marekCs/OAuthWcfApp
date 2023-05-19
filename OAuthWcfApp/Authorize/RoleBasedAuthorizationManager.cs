using System.IO;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Web;

namespace OAuthWcfApp.Authorize
{
    public class RoleBasedAuthorizationManager : ServiceAuthorizationManager
    {
        private readonly JwtTokenHandler _jwtTokenHandler;
        public RoleBasedAuthorizationManager()
        {
            _jwtTokenHandler = new JwtTokenHandler();
        }
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            // Get the action name (e.g. "http://tempuri.org/ITokenService/Authorize")
            var action = operationContext.RequestContext.RequestMessage.Headers.Action;

            // Exclude some actions from access control, because with TokenService you don't yet have a jwtToken (access token) and therefore a role in the header
            if (action.EndsWith("ITokenService/Authorize") || action.EndsWith("ITokenService/Exchange"))
            {
                return true; // allow these actions without access check
            }
            var requestProperty = (HttpRequestMessageProperty)operationContext.IncomingMessageProperties[HttpRequestMessageProperty.Name];
            var authHeader = requestProperty.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var jwtToken = authHeader.Substring("Bearer ".Length);

                // Here we imitate the JwtToken nuget library, which we can't use and had to be programmed manually because Wcf runs on .NET 3.0
                if (_jwtTokenHandler.IsValidToken(jwtToken))
                {
                    var role = _jwtTokenHandler.GetRoleFromToken(jwtToken);

                    // Create an identification with a role
                    var identity = new GenericIdentity("username");

                    // Adding a Role to Identification (for .NET Framework 3.0)
                    var roles = new string[] { role };
                    var principal = new GenericPrincipal(identity, roles);

                    // Setting the current principal
                    Thread.CurrentPrincipal = principal;

                    // If needed, setup a principal for HttpContext (e.g. for ASP.NET), but only if you would like to upgrade to .NET 3.5 and higher
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = principal;
                    }
                    return true;
                }
            }
            else
            {
                SaveLog("JwtToken Expired!");
            }
            SaveLog("authHeader is empty");
            // Unauthorized access - prevent call from going through by returning false.
            return false;
        }

        // Self-logging. Replaced by a robust logging solution and place it also to DI contejner
        private void SaveLog(string message)
        {
            using (StreamWriter writer = new StreamWriter("yourLogPath"))
            {
                writer.WriteLine(message);
            }
        }
    }

}
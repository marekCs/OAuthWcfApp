using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Web;

namespace OAuthWcfApp.Authorize
{
    public class RoleBasedAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            var requestProperty = (HttpRequestMessageProperty)operationContext.IncomingMessageProperties[HttpRequestMessageProperty.Name];
            var authHeader = requestProperty.Headers["Authorization"];

            if ((authHeader != null) && (authHeader != string.Empty))
            {
                var jwtTokenHandler = new JwtTokenHandler();
                if (jwtTokenHandler.IsValidToken(authHeader))
                {
                    var role = jwtTokenHandler.GetRoleFromToken(authHeader);

                    // Create a GenericPrincipal and attach it to the OperationContext.User.
                    // This principal will flow throughout the request.
                    List<string> roles = new List<string>() { role };
                    Thread.CurrentPrincipal = new GenericPrincipal(
                        new GenericIdentity("username"), roles.ToArray());

                    // if needed
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = Thread.CurrentPrincipal;
                    }
                    return true;
                }
            }
            // Unauthorized access - prevent call from going through by returning false.
            return false;
        }
    }

}
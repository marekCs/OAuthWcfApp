using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        private const string _logPath = "C:\\Users\\marek\\Documents\\Logy\\roles.txt";
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            // Get the action name (e.g. "http://tempuri.org/ITokenService/Authorize")
            var action = operationContext.RequestContext.RequestMessage.Headers.Action;

            // Exclude some actions from access check
            if (action.EndsWith("ITokenService/Authorize") || action.EndsWith("ITokenService/Exchange"))
            {
                return true; // allow these actions without access check
            }
            var requestProperty = (HttpRequestMessageProperty)operationContext.IncomingMessageProperties[HttpRequestMessageProperty.Name];
            var authHeader = requestProperty.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var jwtToken = authHeader.Substring("Bearer ".Length);

                // Validace a získání role z JWT tokenu
                var jwtTokenHandler = new JwtTokenHandler();
                if (jwtTokenHandler.IsValidToken(jwtToken))
                {
                    var role = jwtTokenHandler.GetRoleFromToken(jwtToken);
                    string message = "Role from token is: " + role;
                    // Vytvoření identifikace s rolí
                    var identity = new GenericIdentity("username");

                    // Přidání role do identifikace (pro .NET Framework 3.0)
                    var roles = new string[] { role };
                    var principal = new GenericPrincipal(identity, roles);

                    // Nastavení aktuálního principálu
                    Thread.CurrentPrincipal = principal;

                    // Pokud je potřeba, nastavení principálu pro HttpContext (např. pro ASP.NET)
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = principal;
                    }

                    string newMessage = " Current principal role: " + Thread.CurrentPrincipal.IsInRole("Admin");
                    SaveLog(message + newMessage);
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

        private void SaveLog(string message)
        {
            using (StreamWriter writer = new StreamWriter(_logPath))
            {
                writer.WriteLine(message);
            }
        }
    }

}
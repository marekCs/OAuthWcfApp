using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TestWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TokenService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TokenService.svc or TokenService.svc.cs at the Solution Explorer and start debugging.
    public class TokenService : ITokenService
    {
        public string Authorize(string credentials)
        {
            if (credentials == "Ano")
            {
                return "Ano";
            }
            else
            {
                return credentials;
            }
        }
    }
}

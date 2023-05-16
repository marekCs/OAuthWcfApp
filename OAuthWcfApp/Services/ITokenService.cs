using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace OAuthWcfApp.Services
{
    [ServiceContract]
    public interface ITokenService
    {
        [OperationContract]
        string Authorize(string login, string password);

        [OperationContract]
        string Exchange(string authorizationGrant);
    }
}

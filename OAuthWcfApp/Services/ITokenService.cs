using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace OAuthWcfApp.Services
{
    [DataContract]
    public class AuthRequest
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }
    }

    [ServiceContract]
    public interface ITokenService
    {
        [OperationContract]
        string Authorize(AuthRequest request);

        [OperationContract]
        string Exchange(string authorizationGrant);
    }
}

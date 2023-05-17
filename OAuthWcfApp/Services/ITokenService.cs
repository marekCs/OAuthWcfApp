using System.ServiceModel;

namespace OAuthWcfApp.Services
{
    [ServiceContract]
    public interface ITokenService
    {
        [OperationContract]
        string Authorize(string loginPasswordInJsonFormat);

        [OperationContract]
        string Exchange(string authorizationGrant);
    }
}

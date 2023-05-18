using System.ServiceModel;

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

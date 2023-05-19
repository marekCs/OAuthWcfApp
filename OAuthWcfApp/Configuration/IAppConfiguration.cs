namespace OAuthWcfApp.Configuration
{
    public interface IAppConfiguration
    {
        string SecretKey { get; }
        string TokenExpiryMinutes { get; }
        // Feel free to add additional configurations from Web.config
    }
}

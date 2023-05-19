using System.Configuration;

namespace OAuthWcfApp.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration()
        {
            SecretKey = ConfigurationManager.AppSettings["SecretKey"];
            TokenExpiryMinutes = ConfigurationManager.AppSettings["TokenExpiryMinutes"];
            // other configuration data
        }

        public string SecretKey { get; private set; }

        public string TokenExpiryMinutes { get; private set; }
    }
}
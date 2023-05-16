using System;
using System.Collections.Generic;
using System.Web;

namespace OAuthWcfApp.Authorize
{
    public class LoginValidator
    {
        public bool ValidateCredentials(string login, string password)
        {
            // Here you should verify the login credentials against a database or other data source.
            // For simplicity, in this example we accept fixed login credentials.
            return (login == "test" && password == "password");
        }
    }
}
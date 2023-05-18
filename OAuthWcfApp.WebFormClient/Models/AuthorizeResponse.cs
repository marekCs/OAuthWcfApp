using System;
using System.Collections.Generic;
using System.Web;

namespace OAuthWcfApp.WebFormClient.Models
{
    public class AuthorizeResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
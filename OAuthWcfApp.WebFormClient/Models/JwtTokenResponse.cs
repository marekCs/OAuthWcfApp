using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuthWcfApp.WebFormClient.Models
{
    public class JwtTokenResponse
    {
        public string JwtToken { get; set; }
        public string Success { get; set; }
    }
}
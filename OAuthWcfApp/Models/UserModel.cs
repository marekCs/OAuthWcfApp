using System;
using System.Collections.Generic;
using System.Web;

namespace OAuthWcfApp.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRoles Role { get; set; }
    }
}
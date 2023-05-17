using System;
using System.Collections.Generic;
using System.Web;

namespace OAuthWcfApp.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Occupation { get; set; }
        public string Nationality { get; set; }
        public string PassportNumber { get; set; }
        public UserRoles Role { get; set; }
    }
}
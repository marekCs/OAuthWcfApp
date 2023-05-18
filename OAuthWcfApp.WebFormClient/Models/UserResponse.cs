using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuthWcfApp.WebFormClient.Models
{
    public class UserResponse
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
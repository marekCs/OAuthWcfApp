using System;

namespace OAuthWcfApp.Authorize
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class CustomPrincipalPermissionAttribute : Attribute
    {
        private readonly string role;

        public string Role
        {
            get { return role; }
        }

        public CustomPrincipalPermissionAttribute(string role)
        {
            this.role = role;
        }
    }
}
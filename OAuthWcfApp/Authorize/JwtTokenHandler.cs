using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using OAuthWcfApp.Models;

namespace OAuthWcfApp.Authorize
{
    public class JwtTokenHandler
    {
        private string secretKey = ConfigurationManager.AppSettings["SecretKey"];

        public string GenerateToken(string username, UserRoles role)
        {
            var header = new { alg = "HS256", typ = "JWT" };
            var payload = new
            {
                sub = username,
                role = role.ToString(),
                iat = DateTime.Now.Ticks / TimeSpan.TicksPerSecond,
                exp = DateTime.Now.AddMinutes(20).Ticks / TimeSpan.TicksPerSecond
            };

            string stringifiedHeader = SerializeToJson(header);
            string stringifiedPayload = SerializeToJson(payload);
            string signature = CreateHmacSignature(stringifiedHeader, stringifiedPayload);

            return Base64UrlEncode(stringifiedHeader) + "." + Base64UrlEncode(stringifiedPayload) + "." + signature;
        }

        public bool IsValidToken(string token)
        {
            string[] parts = token.Split('.');
            if (parts.Length != 3)
                return false;

            string header = parts[0];
            string payload = parts[1];
            string signature = parts[2];

            string computedSignature = CreateHmacSignature(Base64UrlDecode(header), Base64UrlDecode(payload));
            if (computedSignature != signature)
                return false;

            var payloadJson = DeserializeFromJson<Base64UrlPayload>(Base64UrlDecode(payload));
            long exp = Convert.ToInt64(payloadJson.exp);
            long now = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;

            return now < exp;
        }

        private string CreateHmacSignature(string header, string payload)
        {
            byte[] key = Encoding.UTF8.GetBytes(secretKey);
            byte[] message = Encoding.UTF8.GetBytes(header + "." + payload);

            using (var hmacsha256 = new HMACSHA256(key))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(message);
                return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
            }
        }

        private string Base64UrlEncode(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        private string Base64UrlDecode(string base64Url)
        {
            base64Url = base64Url.Replace('_', '/').Replace('-', '+');
            switch (base64Url.Length % 4)
            {
                case 2: base64Url += "=="; break;
                case 3:
                    base64Url += "="; break;
            }
            var bytes = Convert.FromBase64String(base64Url);
            return Encoding.UTF8.GetString(bytes);
        }

        private string SerializeToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        private T DeserializeFromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        private class Base64UrlPayload
        {
            public string sub { get; set; }
            public long iat { get; set; }
            public long exp { get; set; }
        }
    }
}

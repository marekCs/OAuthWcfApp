using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Xml;
using Newtonsoft.Json;
using OAuthWcfApp.WebFormClient.Models;

namespace OAuthWcfApp.WebFormClient
{
    public partial class Default : System.Web.UI.Page
    {
        [WebMethod()]
        public static AuthorizeResponse AuthorizeUser(string login, string password)
        {
            try
            {
                string soapResponse = GetAuthorizationGrantRequest(login, password);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(soapResponse);

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
                nsmgr.AddNamespace("temp", "http://tempuri.org/");

                XmlNode node = doc.DocumentElement.SelectSingleNode("//s:Body/temp:AuthorizeResponse/temp:AuthorizeResult", nsmgr);
                if (node == null)
                {
                    throw new Exception("AuthorizeResult element not found in the SOAP response.");
                }
                string response = node.InnerXml;
                var jsonResponse = JsonConvert.DeserializeObject<AuthorizeResponse>(response);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Chyba při pokusu odeslat Soap požadavek {ex.Message} {ex.InnerException.Message} {ex.StackTrace}");
            }
        }

        [WebMethod()]
        public static UserResponse GetUserWithJwtToken(string authorizedJwtToken)
        {
            try
            {
                string soapResponse = GetUserAllRequest(authorizedJwtToken);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(soapResponse);

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
                nsmgr.AddNamespace("temp", "http://tempuri.org/");

                XmlNode node = doc.DocumentElement.SelectSingleNode("//s:Body/temp:GetAllUserInfoResponse/temp:GetAllUserInfoResult", nsmgr);
                if (node == null)
                {
                    throw new Exception("GetAllUserInfoResult element not found in the SOAP response.");
                }
                string response = node.InnerXml;
                var jsonResponse = JsonConvert.DeserializeObject<UserResponse>(response);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Chyba při pokusu odeslat Soap požadavek {ex.Message} {ex.InnerException.Message} {ex.StackTrace}");
            }
        }

        [WebMethod()]
        public static JwtTokenResponse ExchangeGrantToken(string authorizationGrant)
        {
            try
            {
                string soapResponse = GetJwtTokenRequest(authorizationGrant);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(soapResponse);

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
                nsmgr.AddNamespace("temp", "http://tempuri.org/");

                XmlNode node = doc.DocumentElement.SelectSingleNode("//s:Body/temp:ExchangeResponse/temp:ExchangeResult", nsmgr);
                if (node == null)
                {
                    throw new Exception("ExchangeResult element not found in the SOAP response.");
                }
                string response = node.InnerXml;
                var jsonResponse = JsonConvert.DeserializeObject<JwtTokenResponse>(response);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Chyba při pokusu odeslat Soap požadavek {ex.Message} {ex.InnerException.Message} {ex.StackTrace}");
            }
        }

        private static string GetUserAllRequest(string jwtToken)
        {
            // Nastavení URL pro vaši WCF službu
            string url = "http://localhost:8089/Services/UserService.svc";

            try
            {
                // Vytvoření nového HttpWebRequest objektu
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("SOAPAction", "http://tempuri.org/IUserService/GetAllUserInfo");
                request.ContentType = "text/xml";
                request.Accept = "text/xml";
                request.Method = "POST";

                // Přidání hlavičky Authorization
                request.Headers.Add("Authorization", "Bearer " + jwtToken);

                // Vytvoření těla SOAP požadavku
                string soapBody = $@"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
    <soap:Body>
        <tem:GetAllUserInfo/>
    </soap:Body>
</soap:Envelope>";
                string soapResponse = SendSoapRequest(request, soapBody);

                return soapResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetJwtTokenRequest(string authorizationGrant)
        {
            // Nastavení URL pro vaši WCF službu
            string url = "http://localhost:8089/Services/TokenService.svc";

            try
            {
                // Vytvoření nového HttpWebRequest objektu
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("SOAPAction", "http://tempuri.org/ITokenService/Exchange");
                request.ContentType = "text/xml";
                request.Accept = "text/xml";
                request.Method = "POST";

                // Vytvoření těla SOAP požadavku
                string soapBody = $@"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
    <soap:Body>
        <tem:Exchange>
            <tem:authorizationGrant>{authorizationGrant}</tem:authorizationGrant>
        </tem:Exchange>
    </soap:Body>
</soap:Envelope>";
                string soapResponse = SendSoapRequest(request, soapBody);

                return soapResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetAuthorizationGrantRequest(string login, string password)
        {
            // Nastavení URL pro vaši WCF službu
            string url = "http://localhost:8089/Services/TokenService.svc";

            try
            {
                // Vytvoření nového HttpWebRequest objektu
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("SOAPAction", "http://tempuri.org/ITokenService/Authorize");
                request.ContentType = "text/xml";
                request.Accept = "text/xml";
                request.Method = "POST";

                // Vytvoření těla SOAP požadavku
                string soapBody = $@"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
    <soap:Body>
        <tem:Authorize>
            <tem:login>{login}</tem:login>
            <tem:password>{password}</tem:password>
        </tem:Authorize>
    </soap:Body>
</soap:Envelope>";
                string soapResponse = SendSoapRequest(request, soapBody);

                return soapResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string SendSoapRequest(HttpWebRequest request, string soapBody)
        {
            // Odeslání SOAP požadavku
            using (Stream stream = request.GetRequestStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(soapBody);
                stream.Write(bytes, 0, bytes.Length);
            }

            // Získání odpovědi
            string soapResponse;
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    soapResponse = reader.ReadToEnd();
                }
            }

            return soapResponse;
        }
    }
}

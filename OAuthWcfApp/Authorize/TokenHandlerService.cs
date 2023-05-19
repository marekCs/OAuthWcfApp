using OAuthWcfApp.Models;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.IO;

namespace OAuthWcfApp.Authorize
{
    public class TokenHandlerService
    {
        private readonly JwtTokenHandler _jwtTokenHandler;
        private readonly string _authorizationGrantsFilePath;
        private readonly string _accessTokensFilePath;
        public TokenHandlerService()
        {
            _jwtTokenHandler = new JwtTokenHandler();
            _authorizationGrantsFilePath = "youirPathForTesting\\authorization_grants.json";
            _accessTokensFilePath = "youirPathForTesting\\access_tokens.json";
        }

        public void StoreAuthorizationGrant(string authorizationGrant, UserModel user)
        {
            Dictionary<string, UserModel> authorizationGrants = LoadDataFromFile<Dictionary<string, UserModel>>(_authorizationGrantsFilePath) ?? new Dictionary<string, UserModel>();
            authorizationGrants[authorizationGrant] = user;
            SaveDataToFile(_authorizationGrantsFilePath, authorizationGrants);
        }

        public string Exchange(string authorizationGrant)
        {
            Dictionary<string, UserModel> authorizationGrants = LoadDataFromFile<Dictionary<string, UserModel>>(_authorizationGrantsFilePath);
            Dictionary<string, UserModel> accessTokens = LoadDataFromFile<Dictionary<string, UserModel>>(_accessTokensFilePath);

            if (authorizationGrants != null && authorizationGrants.ContainsKey(authorizationGrant))
            {
                UserModel user = authorizationGrants[authorizationGrant];
                string accessToken = _jwtTokenHandler.GenerateToken(user.Id.ToString(), user.Role);

                if (accessTokens == null)
                {
                    accessTokens = new Dictionary<string, UserModel>();
                }
                accessTokens[accessToken] = user;
                SaveDataToFile(_accessTokensFilePath, accessTokens);

                authorizationGrants.Remove(authorizationGrant);
                SaveDataToFile(_authorizationGrantsFilePath, authorizationGrants);

                return accessToken;
            }
            else
            {
                return "";
                throw new Exception("Invalid authorization grant");
            }
        }

        public UserModel GetAssociatedUser()
        {
            try
            {
                // Replace this with something from the database, for testing purposes we want to return something that only Admin has access to, but it can be anything not only User info
                Dictionary<string, UserModel> accessTokens = LoadDataFromFile<Dictionary<string, UserModel>>(_accessTokensFilePath);
                UserModel firstUser = null;

                foreach (UserModel user in accessTokens.Values)
                {
                    firstUser = user;
                    break;
                }
                return firstUser;
            }
            catch (Exception ex)
            {
                SaveLog("We cannot return User " + ex.Message);
                return new UserModel { };
                throw new Exception(ex.Message);
            }
            
        }

        public bool ValidateToken(string jwtToken)
        {
            try
            {
                Dictionary<string, UserModel> accessTokens = LoadDataFromFile<Dictionary<string, UserModel>>(_accessTokensFilePath);
                if (jwtToken == null || !accessTokens.ContainsKey(jwtToken))
                {
                    SaveLog("Buď je JwtToken:" + jwtToken + " prázdný a nebo není v seznamu tokenů.");
                    return false;
                    throw new Exception("Buď je JwtToken:" + jwtToken + " prázdný a nebo není v seznamu tokenů.");
                }

                // If the token is not valid, remove it from the list
                if (!_jwtTokenHandler.IsValidToken(jwtToken))
                {
                    accessTokens.Remove(jwtToken);
                    SaveDataToFile(_accessTokensFilePath, accessTokens);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                SaveLog("We cannot validate jwtToken: "+ jwtToken + " " + ex.Message);
                return false;
                throw new Exception(ex.Message);
            }

        }

        public UserModel GetAssociatedUserWithGrant(string authorizationGrant)
        {
            // Reading data from a file (or database) into the dictionary.
            Dictionary<string, UserModel> authorizationGrants = LoadDataFromFile<Dictionary<string, UserModel>>(_authorizationGrantsFilePath) ?? new Dictionary<string, UserModel>();

            // If the dictionary contains the given grant token, return the corresponding user.
            if (authorizationGrants.ContainsKey(authorizationGrant))
            {
                return authorizationGrants[authorizationGrant];
            }

            // If the dictionary does not contain the given grant token, return null.
            return null;
        }


        private void SaveDataToFile<T>(string filePath, T data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private T LoadDataFromFile<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            }
            return default(T);
        }

        // replace it with your own logging system and add it to the DI container
        public void SaveLog(string message)
        {
            using (StreamWriter writer = new StreamWriter("yourLogPath"))
            {
                writer.WriteLine(message, true);
            }
        }
    }
}

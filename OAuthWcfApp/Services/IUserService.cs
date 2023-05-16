﻿using OAuthWcfApp.Models;
using System.ServiceModel;

namespace OAuthWcfApp.Services
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        UserModel GetUser(string accessToken);
    }
}
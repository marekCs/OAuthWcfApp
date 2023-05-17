﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OAuthWcfApp.WebFormClient.TokenClient {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TokenClient.ITokenService")]
    public interface ITokenService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITokenService/Authorize", ReplyAction="http://tempuri.org/ITokenService/AuthorizeResponse")]
        string Authorize(string login, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITokenService/Exchange", ReplyAction="http://tempuri.org/ITokenService/ExchangeResponse")]
        string Exchange(string authorizationGrant);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITokenServiceChannel : OAuthWcfApp.WebFormClient.TokenClient.ITokenService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TokenServiceClient : System.ServiceModel.ClientBase<OAuthWcfApp.WebFormClient.TokenClient.ITokenService>, OAuthWcfApp.WebFormClient.TokenClient.ITokenService {
        
        public TokenServiceClient() {
        }
        
        public TokenServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TokenServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TokenServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TokenServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Authorize(string login, string password) {
            return base.Channel.Authorize(login, password);
        }
        
        public string Exchange(string authorizationGrant) {
            return base.Channel.Exchange(authorizationGrant);
        }
    }
}
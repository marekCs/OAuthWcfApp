﻿using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.Web;

namespace OAuthWcfApp.Authorize
{
    public class CustomHeaderMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

            return httpRequest.Headers["Origin"];
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var origin = (string)correlationState;
            var httpResponse = new HttpResponseMessageProperty();
            reply.Properties.Add(HttpResponseMessageProperty.Name, httpResponse);

            httpResponse.Headers.Add("Access-Control-Allow-Origin", origin);
            httpResponse.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            httpResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                httpResponse.SuppressEntityBody = true;
                httpResponse.StatusCode = HttpStatusCode.OK;
            }
        }
    }
}
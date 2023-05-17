using System;
using System.Collections.Generic;
using System.Web;

namespace CorsModule
{
    public class CorsModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
        }

        public void Dispose()
        {
            // Implementace Dispose() je volitelná.
        }

        private void OnBeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            string origin = context.Request.Headers["Origin"];

            if (!string.IsNullOrEmpty(origin))
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                // Zde můžete přidat další potřebné hlavičky CORS.

                if (context.Request.HttpMethod == "OPTIONS")
                {
                    context.Response.StatusCode = 200;
                    context.Response.End();
                }
            }
        }
    }
}
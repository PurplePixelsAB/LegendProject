using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace DataServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "AuthRcpApiNoId",
            //    routeTemplate: "rcp/{controller}/{action}",
            //    defaults: new { Controller = "Auth" }
            //);
            //config.Routes.MapHttpRoute(
            //    name: "AuthRcpApi",
            //    routeTemplate: "rcp/{controller}/{action}/{id}",
            //    defaults: new { Controller = "Auth", id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "CharacterLearnPower",
                routeTemplate: "API/Character/LearnPower/{characterId}/{power}"
            );

            config.Routes.MapHttpRoute(
                name: "MapIdApi",
                routeTemplate: "API/{controller}/{action}/{mapId}"
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "API/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

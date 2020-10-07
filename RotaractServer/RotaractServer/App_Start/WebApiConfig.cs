using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using RotaractServer.Helpers;
using RotaractServer.Services;
using Unity;
using Unity.Lifetime;

namespace RotaractServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var container = new UnityContainer();
            container.RegisterType<IRotaractRepository, RotaractRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}

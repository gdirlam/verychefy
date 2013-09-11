using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace verychefy {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            /*config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );*/

            System.Web.Mvc.ControllerBuilder.Current.DefaultNamespaces.Add("WebAPI.API.Controllers");
            config.Routes.MapHttpRoute(
                 name: "DefaultApi"
                 , routeTemplate: "Data/Products/{controller}/{id}"
                 , defaults: new { id = RouteParameter.Optional }
             );
        }
    }
}

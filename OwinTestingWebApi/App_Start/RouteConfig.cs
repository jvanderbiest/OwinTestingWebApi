using System.Web.Mvc;
using System.Web.Routing;

namespace OwinTestingWebApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // for mvc redirect to index.html
            routes.IgnoreRoute(""); 
        }
    }
}

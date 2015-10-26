using System;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OwinTestingWebApi;
using OwinTestingWebApi.IoC.Installer;

namespace T2F.WebApi.IoC
{
    public class ApiComponentRegistration
    {
        private static IWindsorContainer _container;

        public static IWindsorContainer Register(HttpConfiguration configuration, Action<IWindsorContainer> containerAction)
        {
            _container = new WindsorContainer();
            containerAction(_container);
            return DoRegistration(configuration);
        }

        public static IWindsorContainer Register(HttpConfiguration configuration)
        {
            _container = new WindsorContainer();
            return DoRegistration(configuration);
        }

        private static IWindsorContainer DoRegistration(HttpConfiguration configuration)
        {
            _container.Install(new ApiControllerInstaller());
            _container.Install(new ServicesInstaller());

            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot(_container));
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_container));

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            return _container;
        }

        public static void Release()
        {
            if (_container != null)
                _container.Dispose();
        }
    }
}
using System;
using System.Web.Http;
using Castle.Windsor;
using Microsoft.Owin;
using Owin;
using OwinTestingWebApi;
using OwinTestingWebApi.IoC;

[assembly: OwinStartup(typeof(Startup))]

namespace OwinTestingWebApi
{
    public partial class Startup
    {
        private IWindsorContainer _container;
        private bool _isTest;

        public void Configuration(IAppBuilder app)
        {
            if (!_isTest) { 
            _container = ApiComponentRegistration.Register(GlobalConfiguration.Configuration);
            }
        }

        public void ConfigureForIntegrationTests(IAppBuilder app, Action<IWindsorContainer> containerAction)
        {
            _isTest = true;
            _container = ApiComponentRegistration.Register(GlobalConfiguration.Configuration, containerAction);
            Configuration(app);
        }
    }
}

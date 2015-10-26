using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Castle.Windsor;
using Microsoft.Owin;
using Owin;
using T2F.WebApi.IoC;

[assembly: OwinStartup(typeof(OwinTestingWebApi.Startup))]

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

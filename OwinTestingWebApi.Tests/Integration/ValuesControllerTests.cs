using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using Owin;

namespace OwinTestingWebApi.Tests.Integration
{
    [TestFixture]
    public class ValuesControllerTests
    {
        private readonly string _url = "http://localhost/api/values";

        internal class Startup1
        {
            public void Configuration(IAppBuilder app)
            {
                var config = new HttpConfiguration();

                new Startup().ConfigureForIntegrationTests(app, config,x => x.Kernel.ComponentModelBuilder.AddContributor(new SingletonEqualizer()));
                GlobalConfiguration.Configuration.Services.Replace(typeof(IAssembliesResolver), new TestWebApiResolver());
                GlobalConfiguration.Configure(WebApiConfig.Register);
            }
        }

        internal class Startup2
        {
            public void Configuration(IAppBuilder app)
            {
                var config = new HttpConfiguration();
                
                new Startup().ConfigureForIntegrationTests(app, config,x => x.Kernel.ComponentModelBuilder.AddContributor(new SingletonEqualizer()));
                GlobalConfiguration.Configure(WebApiConfig.Register);
                GlobalConfiguration.Configuration.EnsureInitialized();
            }
        }

        internal class Startup3
        {
            public void Configuration(IAppBuilder app)
            {
                HttpConfiguration config = new HttpConfiguration();

                new Startup().ConfigureForIntegrationTests(app, config, y => y.Kernel.ComponentModelBuilder.AddContributor(new SingletonEqualizer()));
                // Web API routes

                config.MapHttpAttributeRoutes();

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                app.UseWebApi(config);
                //config.Services.Replace(typeof(IAssembliesResolver), new TestWebApiResolver());
            }
        }

        public TestServer Server { get; set; }

        [Test]
        public async void GetValueTestForStartup5()
        {
            using (var server = TestServer.Create<Startup3>())
            {
                //GlobalConfiguration.Configuration.EnsureInitialized();

                using (var client = new HttpClient(server.Handler))
                {
                    var response = await client.GetAsync(_url);
                    var result = await response.Content.ReadAsAsync<List<string>>();
                    Assert.IsTrue(result.Any());
                }
            }
        }
    }
}

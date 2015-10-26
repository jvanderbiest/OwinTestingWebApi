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
                new Startup().ConfigureForIntegrationTests(app, x => x.Kernel.ComponentModelBuilder.AddContributor(new SingletonEqualizer()));
                GlobalConfiguration.Configuration.Services.Replace(typeof(IAssembliesResolver), new TestWebApiResolver());
                GlobalConfiguration.Configure(WebApiConfig.Register);
            }
        }

        internal class Startup2
        {
            public void Configuration(IAppBuilder app)
            {
                new Startup().ConfigureForIntegrationTests(app, x => x.Kernel.ComponentModelBuilder.AddContributor(new SingletonEqualizer()));
                GlobalConfiguration.Configure(WebApiConfig.Register);
                GlobalConfiguration.Configuration.EnsureInitialized();
            }
        }

        internal class Startup3
        {
            public void Configuration(IAppBuilder app)
            {
                new Startup().ConfigureForIntegrationTests(app, y => y.Kernel.ComponentModelBuilder.AddContributor(new SingletonEqualizer()));
                // Web API routes

                GlobalConfiguration.Configuration.MapHttpAttributeRoutes();

                GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                app.UseWebApi(GlobalConfiguration.Configuration);
                GlobalConfiguration.Configuration.Services.Replace(typeof(IAssembliesResolver), new TestWebApiResolver());
            }
        }

        public TestServer Server { get; set; }

        [Test]
        public async void GetValueTestForStartup1()
        {
            using (var server = TestServer.Create<Startup1>())
            {
                using (var client = new HttpClient(server.Handler))
                {
                    var request = await client.GetAsync(_url);

                    // 404 Error
                    Assert.IsTrue(request.IsSuccessStatusCode);
                }
            }
        }

        [Test]
        public void GetValueTestForStartup3()
        {
            using (var server = TestServer.Create<Startup2>())
            {
                // System.InvalidOperationException : This method cannot be called during the application's pre-start initialization phase.
            }
        }

        [Test]
        public async void GetValueTestForStartup5()
        {
            using (var server = TestServer.Create<Startup3>())
            {
                GlobalConfiguration.Configuration.EnsureInitialized();

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

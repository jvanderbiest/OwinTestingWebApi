using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;
using OwinTestingWebApi.Controllers;

namespace OwinTestingWebApi.Tests
{
    public class TestWebApiResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            return new List<Assembly> { typeof(ValuesApiController).Assembly };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OwinTestingWebApi.Services;

namespace OwinTestingWebApi.Controllers
{
    [RoutePrefix("api")]
    public class ValuesApiController : ApiController
    {
        private readonly ValuesControllerService _valuesControllerService;

        public ValuesApiController(ValuesControllerService valuesControllerService)
        {
            _valuesControllerService = valuesControllerService;
        }

        // GET api/values
        [HttpGet]
        [Route("values")]
        public async Task<IHttpActionResult> Get()
        {
            if (_valuesControllerService == null)
                throw new ArgumentException("valuesControllerService");

            return Ok(new[] { "value1", "value2" });
        }
    }
}

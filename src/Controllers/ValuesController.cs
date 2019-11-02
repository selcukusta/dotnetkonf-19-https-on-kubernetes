using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace samples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            var value = new List<Body>();
            value.Add(new Body(Request.Headers.Any(x => x.Key == "X-Forwarded-Proto"))
            {
                Header = "Scheme",
                Value = Request.Headers.Any(x => x.Key == "X-Forwarded-Proto") ?
                Request.Headers["X-Forwarded-Proto"].ToString() :
                Request.Scheme
            });

            value.Add(new Body(Request.Headers.Any(x => x.Key == "X-Forwarded-Host"))
            {
                Header = "Host",
                Value = Request.Headers.Any(x => x.Key == "X-Forwarded-Host") ?
                Request.Headers["X-Forwarded-Host"].ToString() :
                Request.Host.HasValue ? Request.Host.Value : ""
            });

            Response.Headers.Add("X-Hello", "Y-World!");
            return new JsonResult(value);
        }
    }

    public class Body
    {
        public string Header { get; set; }
        public string Value { get; set; }

        private bool _fromForwardedHeader;
        public string FromForwardedHeader
        {
            get
            {
                return _fromForwardedHeader ? bool.TrueString : bool.FalseString;
            }
        }

        public Body(bool fromForwardedHeader)
        {
            _fromForwardedHeader = fromForwardedHeader;
        }
    }
}

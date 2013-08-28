using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BdzWebsiteUtilities;

namespace BDZService.Controllers
{
    public class RouteController : ApiController
    {
        // GET api/route
        public List<List<RouteDTO>> Get(string fromStation, string toStation, string date, string startTime, string endTime)
        {
            return BDZWebsiteUtilities.GetRoutes(fromStation, toStation, date, startTime, endTime);
        }

        // GET api/route/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/route
        public void Post([FromBody]string value)
        {
        }

        // PUT api/route/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/route/5
        public void Delete(int id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BdzWebsiteUtilities;
using System.Threading.Tasks;

namespace BDZService.Controllers
{
    public class RouteController : ApiController
    {
        // GET api/route
        public List<RouteDTO> Get(string fromStation, string toStation, string date, string startTime, string endTime)
        {
            return BdzWebsiteUtilities.BDZWebsiteUtilities.GetRoutes(fromStation, toStation, date, startTime, endTime);
        }
    }
}

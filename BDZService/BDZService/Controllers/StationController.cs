using BdzWebsiteUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BDZService.Controllers
{
    public class StationController : ApiController
    {
        // GET api/station
        public StationDTO Get(string station, string date)
        {
            return BdzWebsiteUtilities.BDZWebsiteUtilities.GetStation(station, date);
        }
    }
}

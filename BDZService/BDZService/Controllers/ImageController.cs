using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BDZService.Controllers
{
    public class ImageController : ApiController
    {
        // GET api/image
        public string Post([FromBody]string mapHref)
        {
            return BdzWebsiteUtilities.BDZWebsiteUtilities.ParseMap(mapHref);
        }
    }
}

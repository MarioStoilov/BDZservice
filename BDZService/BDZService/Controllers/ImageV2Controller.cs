using BDZWebsiteUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BDZService.Controllers
{
    public class ImageV2Controller : ApiController
    {
        // GET api/imageV2
        public string Post([FromBody]ImageDTO imageDTO)
        {
            //return mapHref;
            return BdzWebsiteUtilities.BDZWebsiteUtilities.ParseMap(imageDTO.mapHref);
        }
    }
}

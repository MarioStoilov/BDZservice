using BdzWebsiteUtilities;
using BDZWebsiteUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BDZService.Controllers
{
    public class PriceV2Controller : ApiController
    {
        // GET api/price
        public PriceDTO Post([FromBody]SessionDTO SessionDTO, string id)
        {
            Cookie cookie = new Cookie("JSESSIONID", SessionDTO.sessionId);
            cookie.Domain = "razpisanie.bdz.bg";
            return BdzWebsiteUtilities.BDZWebsiteUtilities.ParcePrice(id, cookie);
        }
    }
}

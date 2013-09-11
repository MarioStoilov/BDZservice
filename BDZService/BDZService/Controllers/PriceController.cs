using BdzWebsiteUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BDZService.Controllers
{
    public class PriceController : ApiController
    {
        // GET api/price
        public PriceDTO Post([FromBody]string sessionId, string id)
        {
            Cookie cookie = new Cookie("JSESSIONID", sessionId);
            cookie.Domain = "razpisanie.bdz.bg";
            return BdzWebsiteUtilities.BDZWebsiteUtilities.ParcePrice(id, cookie);
        }
    }
}

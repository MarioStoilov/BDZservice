using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BdzWebsiteUtilities;

namespace BDZService.Controllers
{
    public class TrainController : ApiController
    {
        // GET api/train
        public TrainDTO Get(string trainNumber, string date)
        {
            return BdzWebsiteUtilities.BDZWebsiteUtilities.GetTrain(trainNumber, date);
        }
    }
}

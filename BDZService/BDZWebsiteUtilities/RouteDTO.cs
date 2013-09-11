using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdzWebsiteUtilities
{
    public class RouteDTO
    {
        public RouteDTO()
        {
            this.routes = new List<SimpleRouteDTO>();
        }

        public string imageURL { get; set; }
        public string sessionID { get; set; }
        public List<SimpleRouteDTO> routes { get; set; }
    }
}

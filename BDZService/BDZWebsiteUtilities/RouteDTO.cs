using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdzWebsiteUtilities
{
    public class RouteDTO
    {
        public string firstStation { get; set; }
        public string secondStation { get; set; }
        public string train { get; set; }
        public List<string> options { get; set; }
        public string departs { get; set; }
        public string arrives { get; set; }
    }
}

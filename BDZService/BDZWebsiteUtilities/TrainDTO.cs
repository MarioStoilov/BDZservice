using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdzWebsiteUtilities
{
    public class TrainDTO
    {
        public List<TrainSimpleStopDTO> stops { get; set; }
        public List<string> options { get; set; }
    }
}

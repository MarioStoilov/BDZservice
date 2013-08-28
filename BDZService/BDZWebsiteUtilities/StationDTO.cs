using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdzWebsiteUtilities
{
    public class StationDTO
    {
        public List<StationEntryDTO> departs { get; set; }
        public List<StationEntryDTO> arrives { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdzWebsiteUtilities
{
    public class PriceDTO
    {
        public string regularFeeSecondClass { get; set; }
        public string regularFeeFirstClass { get; set; }
              
        public string decreasedFeeSecondClass { get; set; }
        public string decreasedFeeFirstClass { get; set; }
              
        public string decreasedFeeWithFirstIncludedSecondClass { get; set; }
        public string decreasedFeeWithFirstIncludedFirstClass { get; set; }
              
        public string relationalSecondClass { get; set; }
        public string relationalFirstClass { get; set; }
              
        public string bothWaysSecondClass { get; set; }
        public string bothWaysFirstClass { get; set; }
              
        public string groupRegularSecondClass { get; set; }
        public string groupRegularFirstClass { get; set; }
              
        public string groupDecreasedSecondClass { get; set; }
        public string groupDecreasedFirstClass { get; set; }
    }
}

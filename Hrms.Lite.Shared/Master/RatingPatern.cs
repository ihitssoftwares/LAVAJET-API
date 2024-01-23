using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.Master
{
   public class RatingPatern:MasterBase
    {
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }
        public UserBase User { get; set; }
        public List<RatingPatern> RatingPatternList { get; set; }
        public FactorType FactorType { get; set; }
        public List<RatingSlab> RatingSlab { get; set; }

    }

    public class RatingSlab
    {  
        public int Rating { get; set; }
        public string Description { get; set; }
        public int? From { get; set; }
        public int? To { get; set; }
    }
}

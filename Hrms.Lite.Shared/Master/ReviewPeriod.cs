using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.Master
{
   public class ReviewPeriod:MasterBase
    {
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }
        public List<ReviewPeriod> ReviewPeriodList { get; set; }
        public UserBase User { get; set; }

        public string Year { get; set; }
        public DateTime? Fromperiod { get; set; }

        public List<ReviewPeriodLevel> ReviewPeriodLevelList { get; set; }
        public DateTime? Toperiod { get; set; }
        public ReviewPeriodType ReviewPeriodType { get; set; }

    }
    public class ReviewPeriodType : MasterBase
    {

    }
    public class ReviewPeriodLevel
    {
        public int ReviewPeriodLevelCode { get; set; }
        public string Name { get; set; }
        public bool Applicability { get; set; }
        public bool EnableRating { get; set; }
        public bool SelectRecommendatiobn { get; set; }
    }
}

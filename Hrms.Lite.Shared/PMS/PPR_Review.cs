using PPR.Lite.Shared;
using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.PMS
{
    public class PPR_Review : MasterBase
    {
        public UserBase User { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public List<PPR_Review> PPR_ReviewList { get; set; }
        public MasterBase Year { get; set; }
        public int?[] AppraisalType { get; set; }
        public List<PPR_RatingTypeDescription> PPR_RatingTypeDescription { get; set; }

    }
    public class PPR_RatingTypeDescription : MasterBase
    {
        public int Rating { get; set; }
        public string RatingDescription { get; set; }
        public decimal AchivedFrom { get; set; }
        public decimal AchivedTo { get; set; }
    }

}

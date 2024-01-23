using PPR.Lite.Shared;
using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.PMS
{
    public class PMS_Appraisal : MasterBase
    {
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }
        public string AppraisalCategory { get; set; }
        public int CategoryCode { get; set; }
        public string AppraisalType { get; set; }
        public int AppraisalTypeCode { get; set; }
        public DateTime? DOJFrom { get; set; }
        public DateTime? DOJTo { get; set; }
        public UserBase User { get; set; }  
        public List<AppraisalType> AppraisalTypeList { get; set; }
        public List<AuthorityLevel> AuthorityLevelList { get; set; }
        public List<PMS_Appraisal> PMS_AppraisalList { get; set; }
        public PMS_ReviewType PMS_ReviewType { get; set; }
        public PMS_Unit PMS_Unit { get; set; }
        
    }
    public class AppraisalType
    {
        public int AppraisalTypeCode { get; set; }
        public string Name { get; set; }
        public bool Applicability { get; set; }
    }
    public class AuthorityLevel
    {
        public int AuthorityLevelCode { get; set; }
        public string Name { get; set; }
        public bool Applicability { get; set; }
        public bool EnableRating { get; set; }
        public bool SelectRecommendatiobn { get; set; }
    }

    public class PMS_ReviewType :MasterBase
    {

    }
    public class PMS_Unit : MasterBase
    {

    }

}

using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;
namespace PPR.Lite.Shared.Master
{
    public class Designation : MasterBase
    {
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }
        public UserBase User { get; set; }
        public int MRFApproval { get; set; }
        public MasterBase InterviewLevels { get; set; }
        public int LicenseRequired { get; set; }
        public List<Designation> DesignationList { get; set; }
    }
}

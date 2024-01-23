using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.PMS
{
   public class WageGradeMaster:MasterBase
    {
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }
        public UserBase User { get; set; }
        public string Status { get; set; }
        public string EntryType { get; set; }
        public List<WageGradeMaster> WageGradeList { get; set; }
    }
}

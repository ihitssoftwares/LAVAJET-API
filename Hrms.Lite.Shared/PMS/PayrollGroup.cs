using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.PMS
{
   public class PayrollGroup:MasterBase
    {
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }
        public string EntryType { get; set; }
        public List<PayrollGroup> PayrollGroupList { get; set; }
    }
}

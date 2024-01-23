using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.Master
{
   public class FactorType:MasterBase
    {
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }
        public UserBase User { get; set; }

        public List<FactorType> FactorTypeList { get; set; }
    }
}

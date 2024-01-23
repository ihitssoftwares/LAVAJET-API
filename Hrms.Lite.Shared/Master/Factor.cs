using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.Master
{
    public class Factor:MasterBase
    {
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }
        public List<Factor> FactorList { get; set; }
        public UserBase User { get; set; }
        public FactorGroup FactorGroup { get; set; }

        public FactorType FactorType { get; set; }
        public Unit Unit { get; set; }
    }
   
    
    public class Unit : MasterBase
    {

    }
}

using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.DataBank.Generic.Master
{
    public class Skill : MasterBase
    {
         public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }
        public List<Skill> SkillList { get; set; }
        public UserBase User { get; set; }
        public MasterBase SkillType { get; set; }
    }
}

using PPR.Lite.Shared.DataBank.Generic.Master;
using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.Master
{
   public class JobRole:MasterBase
    {
        public int ActiveCount { get; set; }
        public int InActiveCount { set; get; }
        public int PendingCount { set; get; }
        public List<JobRole> JobRoleList { get; set; }
        public UserBase User { get; set; }
        public int?[] DepartmentCode { get; set; }
        public string[] DepartmentName { get; set; }

        public int?[] DesignationCode { get; set; }
        public string[] DesignationName { get; set; }

        public int?[] LocationCode { get; set; }
        public string[] LocationName { get; set; }

        public int?[] GradeCode { get; set; }
        public string[] GradeName { get; set; }

        public int?[] APPLICABILITY { get; set; }
    }
}

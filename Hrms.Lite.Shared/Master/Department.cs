using PPR.Lite.Shared.General;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace PPR.Lite.Shared.Master
{
    public class Department:MasterBase
    {
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }
        public List<Department> DepartmentList { get; set; }
        public UserBase User { get; set; }
     
    }
}

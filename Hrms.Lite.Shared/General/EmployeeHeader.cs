using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.General
{
    public class EmployeeHeader
    {
        public Guid EmployeeGuid { get; set; }
        public int EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string ReportingManager { get; set; }
        public string EmployeeAbsoluteUri { get; set; }
        public string EmployeeId { get; set; }
        public Designation Designation { get; set; }
        public Department Department { get; set; }
        public Location Location { get; set; }
        public Category Category { get; set; }
        public string ReportingManagerID { get; set; }
        public Designation ReportingManagerDesignation { get; set; }
        public string LWD { get; set; }

    }
}

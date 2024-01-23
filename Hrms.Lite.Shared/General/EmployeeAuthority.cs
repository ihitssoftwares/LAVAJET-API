using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.General
{
    public class EmployeeAuthority
    {
        public string Level { get; set; }
        public string AuthorityName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AbsoluteUri { get; set; }
        public string EmployeeID { get; set; }
        public string Designation { get; set; }
        public List<EmployeeAuthority> EmployeeAuthorityList { get; set; }

    }
}

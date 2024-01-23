using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PPR.Lite.Shared.DataBank.DTO
{
    public class AuthUser
    {
        public UserBase User { get; set; }
        public string CompanyName { get; set; }
        public int CompanyID { get; set; }
        public int UserCode { get; set; }
       
        public Guid EmployeeGI { get; set; }
        public string EmployeeID { get; set; }
        public int EmployeeCode { get; set; }
        
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string ImageUrl { get; set; }
        [IgnoreDataMember]
        public string PasswordHash { get; set; }
        [IgnoreDataMember]
        public string PasswordSalt { get; set; }
        public string Token { get; set; }
        public string UserType { get; set; }
        public bool IsActive { get; set; }
        public string UserRole { get; set; }
        public int UserRoleCode { get; set; }
        public bool IsNewLogin { get; set; }
        public string DashBoardRoute { get; set; }
        public string GroupCompanyName { get; set; }
        public string GroupCompanyShortName { get; set; }
    }
    public class ApplicantUser
    {
        public UserBase User { get; set; }
        public Guid ApplicantGI { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Country { get; set; }
        public string ImageUrl { get; set; }
        [IgnoreDataMember]
        public string Resume { get; set; }
        [IgnoreDataMember]
        public string PasswordHash { get; set; }
        [IgnoreDataMember]
        public string PasswordSalt { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}

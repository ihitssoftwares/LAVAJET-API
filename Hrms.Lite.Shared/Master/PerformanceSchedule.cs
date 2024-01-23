using PPR.Lite.Shared.General;
using PPR.Lite.Shared.PMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.Master
{
   public class PerformanceSchedule
    {
        public Guid EmployeeGI { get; set; }
        public Guid AppraisalGI { get; set; }
        public int EmployeeCode { get; set; }

        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Grade { get; set; }
        public DateTime DOJ { get; set; }
        public MasterBase AppraisalRole { get; set; }
        public string Self { get; set; }
        public string AuthLevelDetails { get; set; }
        public int Weightage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Comments { get; set; }
        public int CombinationCode { get; set; }
        public int AppraisalCode { get; set; }
        public int ReviewPeriodCode { get; set; }
        public List<PerformanceSchedule> PerformanceScheduleList { get; set; }
        public UserBase User { get; set; }
        public Filter Filter { get; set; }
        public int TotalRecords { get; set; }
        public int Active { get; set; }
        public string status { get; set; }
        


    }
    public class AuthoritySettings
    {
        //public PMS.AuthorityLevel AuthorityLevel { get; set; }
        public MasterBase EmployeeAuthority { get; set; }
    }
    public class AppraisalEmployeeAuthoritySettings
    {
        public int AppraisalCode { get; set; }
        public Guid EmployeeGI { get; set; }
        public bool SelfLevel { get; set; }
        public MasterBase AppraisalRole { get; set; }
        public Filter Filter { get; set; }
        public UserBase UserBase { get; set; }
        public PMS.AuthorityLevel AuthorityLevel { get; set; }
        public string Mode { get; set; }
        public PMS_Appraisal PMS_Appraisal { get; set; }
        public List<PerformanceSchedule> Appraisal_EmployeeList { get; set; }
        public List<AuthoritySettings> AuthoritySettingsList { get; set; }
    }
}

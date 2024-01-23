using PPR.Lite.Shared.Master;
using PPR.Lite.Shared.DataBank.DTO;

using PPR.Lite.Shared.General;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.General
{
    public interface ICommonRepository
    {
        Task<ResponseEntity> CheckEmptyDropDown(string Mode, UserBase user);

        Task<EmployeeHeader> GetEmployeeCard(int EmpCode, UserBase user);
        Task<EmployeeHeader> GetEmployeeHeader(Guid EmployeeGI, UserBase user);
        Task<List<Dropdown>> GetDurationWiseFilterDropDown(UserBase user);
        Task<ResponseEntity<Int32>> GetCurrentLeavePeriod( UserBase user);
        Task<ResponseEntity<Int32>> GetCurrentLFinancialYear(UserBase user);
        Task<AddOnClassification> AddOnClassification_Applicability(UserBase User);
        // Task<EmployeeAuthority> GetEmployeeAuthorityDetails( UserBase user);
        //Task<ResponseEntity> GetLeaveValidation(LeaveApplication Details, UserBase user);
        Task<List<EmployeeAuthority>> GetEmployeeAuthorityDetails(UserBase user);
     
        Task<ResponseEntity<Int32>> GetDisplayOrder(UserBase user, string Master);
        Task<ResponseEntity<string>> GetReferenceNumber(UserBase user, string TransactionName);
        Task<ResponseEntity<Int32>> GetCurrentMonthDropdown(UserBase user);
        Task<DateAndMonth> GetCurrentMonthDetails(UserBase user);
        Task<DateAndMonth> GetSelectedMonthDates(UserBase user, int MonthCode);
        Task<ExcelPassword> GetExcelPassword(UserBase user,string ReportName, string Type);
        public Task<ResponseEntity> ValidateExcelPassword(string Password,string ReportName, string Type, UserBase user);
        Task<ResponseEntity> MRFRequest_Applicability(Guid EmployeeGI, UserBase user);
        Task<UserRole> UserRole_Actions(UserBase User);
       //Task<AttendanceStatus> GetAttendanceStatusType(UserBase User, int AttendanceStatusCode);
       // public Task<PreEnrollment> GetPFMaxLimit(UserBase user);
        public  Task<ResponseEntity> GetSalaryPeriodLockDatewiseValidation(UserBase user, string SeletedDate);
        public  Task<ResponseEntity> GetMasterInactiveValidation(string Master, int Code, UserBase user);
        Task<ResponseEntity<Int32>> GetAttendanceStatusCode(UserBase user, String LEAVETYPE);
    }
}

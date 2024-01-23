using PPR.Lite.Api.Helper;
using PPR.Lite.Repository.IRepository.General;

using PPR.Lite.Shared.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPR.Lite.Api.Controllers.General
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        #region Private properties
        private readonly ICommonRepository _repo;
        #endregion
        #region Constructor        
        public CommonController(ICommonRepository repo)
        {
            _repo = repo;
        }
        #endregion

        #region Get
        [HttpGet("CheckEmptyDropDown")]
        public async Task<IActionResult> CheckEmptyDropDown(string Mode)
        {
            return Ok(await _repo.CheckEmptyDropDown(Mode, UserHelper.GetUser(User)));
        }
        [HttpGet("GetEmployeeCard")]
        public async Task<IActionResult> GetEmployeeCard(int EmpCode)
        {
            return Ok(await _repo.GetEmployeeCard(EmpCode, UserHelper.GetUser(User)));
        }
        [HttpGet("GetEmployeeHeader")]
        public async Task<IActionResult> GetEmployeeHeader(Guid EmployeeGI)
        {
            return Ok(await _repo.GetEmployeeHeader(EmployeeGI, UserHelper.GetUser(User)));
        }
        [HttpGet("GetDurationWiseFilterDropDown")]
        public async Task<IActionResult> GetDurationSlab(Guid EmployeeGI)
        {
            return Ok(await _repo.GetDurationWiseFilterDropDown(UserHelper.GetUser(User)));
        }
        [HttpGet("AddOnClassification_Applicability")]
        public async Task<IActionResult> AddOnClassification_Applicability()
        {
            return Ok(await _repo.AddOnClassification_Applicability(UserHelper.GetUser(User)));
        }
        #region Leave
        [HttpGet("GetCurrentLeavePeriod")]
        public async Task<IActionResult> GetCurrentLeavePeriod()
        {
            return Ok(await _repo.GetCurrentLeavePeriod(UserHelper.GetUser(User)));
        }
        //[HttpPost("GetLeaveValidation")]
        //public async Task<IActionResult> GetLeaveValidation([FromBody] LeaveApplication Details)
        //{

        //    return Ok(await _repo.GetLeaveValidation(Details, UserHelper.GetUser(User)));
        //}
        #endregion
        [HttpGet("GetEmployeeAuthorityDetails")]
        public async Task<IActionResult> GetEmployeeAuthorityDetails()
        {
            return Ok(await _repo.GetEmployeeAuthorityDetails(UserHelper.GetUser(User)));
        }
        #region FinancialYear
        [HttpGet("GetCurrentLFinancialYear")]
        public async Task<IActionResult> GetCurrentLFinancialYear()
        {
            return Ok(await _repo.GetCurrentLFinancialYear(UserHelper.GetUser(User)));
        }
        #endregion
        #region DisplayOrder
        [HttpGet("GetDisplayOrder")]
        public async Task<IActionResult> GetDisplayOrder(string Master)
        {
            return Ok(await _repo.GetDisplayOrder(UserHelper.GetUser(User),Master));
        }
        #endregion
        #region RefNumber
        [HttpGet("GetReferenceNumber")]
        public async Task<IActionResult> GetReferenceNumber(string TransactionName)
        {
            return Ok(await _repo.GetReferenceNumber(UserHelper.GetUser(User), TransactionName));
        }
        #endregion

        [HttpGet("GetCurrentMonthDropdown")]
        public async Task<IActionResult> GetCurrentMonthDropdown()
        {
            return Ok(await _repo.GetCurrentMonthDropdown(UserHelper.GetUser(User)));
        }
        [HttpGet("GetCurrentMonthDetails")]
        public async Task<IActionResult> GetCurrentMonthDetails()
        {
            return Ok(await _repo.GetCurrentMonthDetails(UserHelper.GetUser(User)));
        }
        [HttpGet("GetSelectedMonthDates")]
        public async Task<IActionResult> GetSelectedMonthDates(int MonthCode)
        {
            return Ok(await _repo.GetSelectedMonthDates(UserHelper.GetUser(User), MonthCode));
        }
        #endregion
        #region Excel Password
        [HttpGet("GetExcelPassword")]
        public async Task<IActionResult> GetExcelPassword(string ReportName, string Type)
        {
            var obj = await _repo.GetExcelPassword(UserHelper.GetUser(User), ReportName, Type);
            string Password = Cryptography.Decrypt(obj.Password, true);
            obj.Password = Password;
           return Ok(obj);
        }
        //[HttpPost("ValidateExcelPassword")]
        //public async Task<IActionResult> ValidateExcelPassword([FromBody] ExcelPassword obj)
        //{
        //    string Password = Cryptography.Encrypt(obj.Password, true);
        //   return Ok(await _repo.ValidateExcelPassword(Password,obj.ReportName, obj.Type, UserHelper.GetUser(User)));
        //}
        #endregion

        [HttpGet("MRFRequest_Applicability")]
        public async Task<IActionResult> MRFRequest_Applicability(Guid EmployeeGI)
        {
            return Ok(await _repo.MRFRequest_Applicability(EmployeeGI,UserHelper.GetUser(User)));
        }
        #region User Actions
        [HttpGet("UserRole_Actions")]
        public async Task<IActionResult> UserRole_Actions()
        {
            return Ok(await _repo.UserRole_Actions(UserHelper.GetUser(User)));
        }
        #endregion
        #region AttendanceStatusType
        //[HttpGet("GetAttendanceStatusType")]
        //public async Task<IActionResult> GetAttendanceStatusType(int AttendanceStatusCode)
        //{
        //    return Ok(await _repo.GetAttendanceStatusType(UserHelper.GetUser(User),  AttendanceStatusCode));
        //}
        #endregion

        //[HttpGet("GetPFMaxLimit")]//PF
        //public async Task<IActionResult> GetPFMaxLimit(int SalaryStructureCode, int Gross)
        //{

        //    return Ok(await _repo.GetPFMaxLimit(UserHelper.GetUser(User)));
        //}
        [HttpGet("GetSalaryPeriodLockDatewiseValidation")]
        public async Task<IActionResult> GetSalaryPeriodLockDatewiseValidation( string SeletedDate)
        {
            return Ok(await _repo.GetSalaryPeriodLockDatewiseValidation( UserHelper.GetUser(User), SeletedDate));
        }
        [HttpGet("GetMasterInactiveValidation")]
        public async Task<IActionResult> GetMasterInactiveValidation(string Master, int Code)
        {

            return Ok(await _repo.GetMasterInactiveValidation(Master,  Code, UserHelper.GetUser(User)));
        }
        [HttpGet("GetAttendanceStatusCode")]
        public async Task<IActionResult> GetAttendanceStatusCode(String LEAVETYPE)//ShortName FROM FIXED.mstAttendanceStatusType (COFF,OD,WFH)
        {

            return Ok(await _repo.GetAttendanceStatusCode( UserHelper.GetUser(User),  LEAVETYPE));
        }
    }
}

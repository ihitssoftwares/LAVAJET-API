using PPR.Lite.Infrastructure.Helpers;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Repository.IRepository.General;

using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using PPR.Lite.Shared.Master;
using Hrms.Lite.Shared.Master;

namespace PPR.Lite.Repository.Repository.General
{
    public class CommonRepository : ICommonRepository
    {
        private readonly ISqlConnectionProvider _sql;

        public CommonRepository(ISqlConnectionProvider sql)
        {
            _sql = sql;
        }
        async Task<ResponseEntity> ICommonRepository.CheckEmptyDropDown(string Mode, UserBase user)
        {
            SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
            SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

            IDbDataParameter[] sqlParameters =
            {
              new SqlParameter("@Mode",Mode),
               new SqlParameter("@CompanyGI",user.CompanyGuid),
                 new SqlParameter("@UserGI",user.UserGuid),
            ResponseKey,ResponseStatus
            };
            await _sql.ExecuteNonQueryAsync("[HRMS].[SP_GET_EMPTY_DROPDOWN_DETAILS]", sqlParameters, CommandType.StoredProcedure);
            return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
        }
        public async Task<EmployeeHeader> GetEmployeeCard(int EmpCode, UserBase User)
        {
            IDbDataParameter[] sqlparameters =
            {
                new SqlParameter("@CompanyGI",User.CompanyGuid),
                new SqlParameter("@UserGuid",User.UserGuid),
                new SqlParameter("@CODE",EmpCode)
            };
            var ds = await _sql.GetDataSetAsync("[HRMS].[SP_GET_EMPLOYEE_SEARCH_EMPLOYEE_DETAILS]", sqlparameters);
            return PopulateEmployeeCardDetails(ds);
        }

        private EmployeeHeader PopulateEmployeeCardDetails(DataSet ds)
        {
            EmployeeHeader model = new EmployeeHeader();

            model.Designation = new Shared.Master.Designation();
            model.Department = new Shared.Master.Department();
            model.Location = new Shared.Master.Location();
            model.Category = new Shared.Master.Category();

            if ((ds.Tables[0]?.Rows?.Count ?? 0) > 0)
            {

                model.EmployeeAbsoluteUri = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["AbsoluteUri"]);
                model.EmployeeGuid = MakeSafe.ToSafeGuid(ds.Tables[0].Rows[0]["EmployeeGI"]);
                model.EmployeeId = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["USERCODE"]); ;
                model.EmployeeName = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["FULLNAME"]);
                model.Designation.Name = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["DESIGNATION"]);
                model.Department.Name = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["DEPARTMENT"]);
                model.Location.Name = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["LOCATION"]);
                model.Category.Name = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["CATEGORY"]);
                model.ReportingManager = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["ReportingManeger"]);
                model.LWD = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["LWD"]);
                return model;
            }
            return null;
        }
        public async Task<EmployeeHeader> GetEmployeeHeader(Guid EmployeeGI, UserBase user)
        {
            IDbDataParameter[] sqlparameters =
           {
                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),
                new SqlParameter("@EmployeeGI",EmployeeGI)
            };
            //var ds = await _sql.GetDataSetAsync("[HRMS].[SP_ASSET_GET_ASSET_LOG_LIST]", sqlparameters);
            var ds = await _sql.GetDataSetQueryAsync("SELECT * FROM [HRMS].[FN_GET_EMPLOYEE_BASIC_DETAILS] ('" + user.CompanyGuid + "','" + user.UserGuid + "','" + EmployeeGI + "')", CommandType.Text);
            return PopulateEmployeeHeader(ds);
        }
        private EmployeeHeader PopulateEmployeeHeader(DataSet ds)
        {
            var item = new EmployeeHeader();
            item.Designation = new Shared.Master.Designation();
            item.Department = new Shared.Master.Department();
            item.Location = new Shared.Master.Location();
            item.Category = new Shared.Master.Category();
            item.ReportingManagerDesignation = new Shared.Master.Designation();

            if ((ds.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                item.EmployeeAbsoluteUri = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["AbsoluteUri"]);
                //item.EmployeeHeader.EmployeeGuid = MakeSafe.ToSafeGuid(ds.Tables[0].Rows[0]["EmployeeGI"]);
                item.EmployeeGuid = MakeSafe.ToSafeGuid(ds.Tables[0].Rows[0]["EMPLOYEEGI"]);                
                item.EmployeeId = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["USERCODE"]);
                item.EmployeeCode = MakeSafe.ToSafeInt32(ds.Tables[0].Rows[0]["EMPCODE"]);
                item.EmployeeName = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["FULLNAME"]);
                item.Designation.Name = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["DESIGNATION"]);
                item.Department.Name = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["DEPARTMENT"]);
                item.Location.Name = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["LOCATION"]);
                item.Category.Name = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["CATEGORY"]);
                item.ReportingManager = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["REPORTING_PERSON"]);
                item.ReportingManagerID = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["ReportingManagerID"]);
                item.ReportingManagerDesignation.Name = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["ReportingManagerDesignation"]);
                return item;
            }
            return null;
        }

        public async Task<List<Dropdown>> GetDurationWiseFilterDropDown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid)

                };
            var dataTable = await _sql.GetDataTableAsync("[HRMS].[SP_DISCIPLINARY_INCIDENT_LIST_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        private List<Dropdown> PopulateDropDownDetails(DataTable dt)
        {

            List<Dropdown> DropList = new List<Dropdown>();
            foreach (DataRow dr in dt.Rows)
            {
                DropList.Add(new Dropdown
                {
                    Code = MakeSafe.ToSafeInt32(dr["Code"]),
                    Name = MakeSafe.ToSafeString(dr["Name"])
                });
            }
            return DropList;
        }
        public async Task<ResponseEntity<Int32>> GetCurrentLeavePeriod(UserBase user)
        {
            SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
            SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
            SqlParameter LeavePeriod = new SqlParameter { ParameterName = "@LeavePeriod", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            IDbDataParameter[] sqlParameters =
            {


                new SqlParameter("@UserGI", user.UserGuid),
                new SqlParameter("@CompanyGI", user.CompanyGuid),
                 ResponseKey,ResponseStatus,LeavePeriod
            };
            await _sql.ExecuteNonQueryAsync("[HRMS].[SP_GET_CURRENT_LEAVE_PERIOD]", sqlParameters, CommandType.StoredProcedure);
            return new ResponseEntity<Int32>(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value), Convert.ToInt32(LeavePeriod.Value));
        }
        public async Task<AddOnClassification> AddOnClassification_Applicability(UserBase User)
        {
            IDbDataParameter[] sqlparameters =
            {
             new SqlParameter("@CompanyGuid",User.CompanyGuid),
            new SqlParameter("@UserGuid",User.UserGuid)
            };
            var ds = await _sql.GetDataSetAsync("[HRMS].[SP_GET_ADD_ON_APPLICABILITY]", sqlparameters);
            return PopulateAddOnClassification_Applicability(ds);

        }
        private AddOnClassification PopulateAddOnClassification_Applicability(DataSet ds)
        {
            AddOnClassification model = new AddOnClassification();


            if ((ds.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                model.AddOnClassification_Applicability = MakeSafe.ToSafeBool(ds.Tables[0].Rows[0]["AddOnClassification"]);
                model.DivisionExists = MakeSafe.ToSafeBool(ds.Tables[0].Rows[0]["DivisionExists"]);
                model.SectionExists = MakeSafe.ToSafeBool(ds.Tables[0].Rows[0]["SectionExists"]);
                model.EmploymentTypeExists = MakeSafe.ToSafeBool(ds.Tables[0].Rows[0]["EmploymentTypeExists"]);
                model.WorkLocationExists = MakeSafe.ToSafeBool(ds.Tables[0].Rows[0]["WorkLocationExists"]);
                model.WageGradeExists = MakeSafe.ToSafeBool(ds.Tables[0].Rows[0]["WageGradeExists"]);
            }

            return model;
        }

        //public async Task<EmployeeAuthority> GetEmployeeAuthorityDetails(UserBase user)
        //{
        public async Task<List<EmployeeAuthority>> GetEmployeeAuthorityDetails(UserBase user)

        {
            IDbDataParameter[] sqlParameters =
          {
            new SqlParameter("@UserGI",user.UserGuid),
            new SqlParameter("@CompanyGI",user.CompanyGuid)
            };
            var ds = await _sql.GetDataSetAsync("[ESSP].[SP_LEAVE_AUTHORITY_DETAILS]", sqlParameters);
            return PopulateEmployeeAuthorityDetails(ds);
        }

        //private EmployeeAuthority PopulateEmployeeAuthorityDetails(DataSet ds)
        //{
        //    EmployeeAuthority model = new EmployeeAuthority();
        //   List<EmployeeAuthority> list = new List<EmployeeAuthority>();

        private List<EmployeeAuthority> PopulateEmployeeAuthorityDetails(DataSet ds)
        {
            List<EmployeeAuthority> list = new List<EmployeeAuthority>();

            if ((ds.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var item2 = new EmployeeAuthority();
                    item2.Level = MakeSafe.ToSafeString(row["Level"]);
                    item2.AuthorityName = MakeSafe.ToSafeString(row["AuthEmployee"]);
                    item2.Email = MakeSafe.ToSafeString(row["Email"]);
                    item2.PhoneNumber = MakeSafe.ToSafeString(row["PhoneNumber"]);
                    item2.AbsoluteUri = MakeSafe.ToSafeString(row["AbsoluteUri"]); 
                    item2.EmployeeID = MakeSafe.ToSafeString(row["EmployeeID"]);
                    item2.Designation = MakeSafe.ToSafeString(row["Designation"]);
                    list.Add(item2);
                }
            }
            //model.EmployeeAuthorityList = list;

            //return model;
            return list;

        }
        public async Task<ResponseEntity<Int32>> GetCurrentLFinancialYear(UserBase user)
        {
            SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
            SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
            SqlParameter FinancialYear = new SqlParameter { ParameterName = "@FinancialYearCode", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            IDbDataParameter[] sqlParameters =
            {


                new SqlParameter("@UserGI", user.UserGuid),
                new SqlParameter("@CompanyGI", user.CompanyGuid),
                 ResponseKey,ResponseStatus,FinancialYear
            };
            await _sql.ExecuteNonQueryAsync("[HRMS].[SP_FINANCIAL_YEAR_GET_CURRENT_VALUE]", sqlParameters, CommandType.StoredProcedure);
            return new ResponseEntity<Int32>(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value), Convert.ToInt32(FinancialYear.Value));
        }
        #region DisplayOrder
        public async Task<ResponseEntity<Int32>> GetDisplayOrder(UserBase user, string Master)
        {
            SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
            SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
            SqlParameter DisplayOrder = new SqlParameter { ParameterName = "@DisplayOrder", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            IDbDataParameter[] sqlParameters =
            {


                new SqlParameter("@MASTER", Master),
                new SqlParameter("@CompanyGI", user.CompanyGuid),
                 ResponseKey,ResponseStatus,DisplayOrder
            };
            await _sql.ExecuteNonQueryAsync("[HRMS].[SP_GET_DISPLAY_ORDER]", sqlParameters, CommandType.StoredProcedure);
            return new ResponseEntity<Int32>(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value), Convert.ToInt32(DisplayOrder.Value));
        }
        #endregion
        #region RefNumber
        public async Task<ResponseEntity<string>> GetReferenceNumber(UserBase user, string TransactionName)
        {
            SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
            SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
            SqlParameter NewID = new SqlParameter { ParameterName = "@NEWID", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
            IDbDataParameter[] sqlParameters =
            {


                new SqlParameter("@TransactionName", TransactionName),
                new SqlParameter("@CompanyGI", user.CompanyGuid),
                 ResponseKey,ResponseStatus,NewID
            };
            await _sql.ExecuteNonQueryAsync("[HRMS].[SP_GET_REFERANCE_ID]", sqlParameters, CommandType.StoredProcedure);
            return new ResponseEntity<string>(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value), Convert.ToString(NewID.Value));
        }
        #endregion

       // public async Task<ResponseEntity> GetLeaveValidation(LeaveApplication leaveDetails, UserBase user)
       // {
       //
         //   SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 250, Direction = ParameterDirection.Output };
        //    SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
         //   IDbDataParameter[] sqlParameters =
        //    {
        //    new SqlParameter("@JSONDATA",JsonHelper.ToJson(leaveDetails)),
        //    new SqlParameter("@UserGI",user.UserGuid),
        //    new SqlParameter("@CompanyGI",user.CompanyGuid),
        //   ResponseKey,ResponseStatus
        //    };
         //   await _sql.ExecuteNonQueryAsync("[ESSP].[SP_LEAVE_APPLICATION_VALIDATION]", sqlParameters, CommandType.StoredProcedure);
       //  //   return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
       // }
        public async Task<ResponseEntity<Int32>> GetCurrentMonthDropdown(UserBase user)
        {
            SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
            SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
            SqlParameter Month = new SqlParameter { ParameterName = "@Month", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            IDbDataParameter[] sqlParameters =
            {


                new SqlParameter("@UserGI", user.UserGuid),
                new SqlParameter("@CompanyGI", user.CompanyGuid),
                 ResponseKey,ResponseStatus,Month
            };
            await _sql.ExecuteNonQueryAsync("[HRMS].[SP_GET_CURRENT_MONTH]", sqlParameters, CommandType.StoredProcedure);
            return new ResponseEntity<Int32>(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value), Convert.ToInt32(Month.Value));
        }

        public async Task<DateAndMonth> GetSelectedMonthDates(UserBase user,int MonthCode)
        {
            SqlParameter FromDate = new SqlParameter { ParameterName = "@FROMDATE", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Output };
            SqlParameter ToDate = new SqlParameter { ParameterName = "@TODATE", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Output };
            SqlParameter NextMonthCode = new SqlParameter { ParameterName = "@NXT_MNTH_CODE", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            SqlParameter Processed_NextMonthCode = new SqlParameter { ParameterName = "@NXT_PROC_MNTH_CODE", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            SqlParameter PreviousMonthCode = new SqlParameter { ParameterName = "@PRV_MNTH_CODE", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            SqlParameter Processed_PreviousMonthCode = new SqlParameter { ParameterName = "@PRV_PROC_MNTH_CODE", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };


            try
            {
                IDbDataParameter[] sqlParameters =
                {


                new SqlParameter("@UserGI", user.UserGuid),
                new SqlParameter("@CompanyGI", user.CompanyGuid),
                  new SqlParameter("@MONTHCODE",MonthCode),
                 FromDate,ToDate,NextMonthCode,Processed_NextMonthCode,PreviousMonthCode,Processed_PreviousMonthCode
            };
                await _sql.GetDataSetAsync("[HRMS].[SP_GET_DATES_WITH_MONTH_CODE]", sqlParameters);
                var item = new DateAndMonth();
                item.FromDate = MakeSafe.ToSafeDateTime(FromDate.Value);
                item.ToDate = MakeSafe.ToSafeDateTime(ToDate.Value);
                item.NextMonthCode = MakeSafe.ToSafeInt32(NextMonthCode.Value);
                item.Processed_NextMonthCode = MakeSafe.ToSafeInt32(Processed_NextMonthCode.Value);
                item.PreviousMonthCode = MakeSafe.ToSafeInt32(PreviousMonthCode.Value);
                item.Processed_PreviousMonthCode = MakeSafe.ToSafeInt32(Processed_PreviousMonthCode.Value);
                return item;
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        public async Task<DateAndMonth> GetCurrentMonthDetails(UserBase user)
        {
            SqlParameter Month = new SqlParameter { ParameterName = "@Month", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            SqlParameter NextMonthCode = new SqlParameter { ParameterName = "@NXT_MNTH_CODE", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            SqlParameter Processed_NextMonthCode = new SqlParameter { ParameterName = "@NXT_PROC_MNTH_CODE", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            SqlParameter PreviousMonthCode = new SqlParameter { ParameterName = "@PRV_MNTH_CODE", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            SqlParameter Processed_PreviousMonthCode = new SqlParameter { ParameterName = "@PRV_PROC_MNTH_CODE", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };

            SqlParameter FROMDATE = new SqlParameter { ParameterName = "@FROMDATE", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Output };
            SqlParameter TODATE = new SqlParameter { ParameterName = "@TODATE", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Output };
            SqlParameter Year = new SqlParameter { ParameterName = "@Year", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            SqlParameter FY = new SqlParameter { ParameterName = "@FY", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            IDbDataParameter[] sqlParameters =
            {


                new SqlParameter("@UserGI", user.UserGuid),
                new SqlParameter("@CompanyGI", user.CompanyGuid),
                Month,NextMonthCode,Processed_NextMonthCode,PreviousMonthCode,Processed_PreviousMonthCode,FROMDATE,TODATE,Year,FY
            };
            await _sql.GetDataSetAsync("[HRMS].[SP_GET_CURRENT_MONTH_DETAILS]", sqlParameters);
            var item = new DateAndMonth();
            item.MonthCode = MakeSafe.ToSafeInt32(Month.Value);
            item.NextMonthCode = MakeSafe.ToSafeInt32(NextMonthCode.Value);
            item.Processed_NextMonthCode = MakeSafe.ToSafeInt32(Processed_NextMonthCode.Value);
            item.PreviousMonthCode = MakeSafe.ToSafeInt32(PreviousMonthCode.Value);
            item.Processed_PreviousMonthCode = MakeSafe.ToSafeInt32(Processed_PreviousMonthCode.Value);
            item.FromDate = MakeSafe.ToSafeDateTime(FROMDATE.Value);
            item.ToDate = MakeSafe.ToSafeDateTime(TODATE.Value);
            item.YearCode = MakeSafe.ToSafeInt32(Year.Value);
            item.FyCode = MakeSafe.ToSafeInt32(FY.Value);
            return item;
        }
        #region Excel Password
        public async Task<ExcelPassword> GetExcelPassword(UserBase user, string ReportName, string Type)
        {
            SqlParameter Password = new SqlParameter { ParameterName = "@Password", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
            SqlParameter NeedPassword = new SqlParameter { ParameterName = "@NeedPassword", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

            IDbDataParameter[] sqlParameters =
           {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                 new SqlParameter("@ReportName",ReportName),
                 new SqlParameter("@Type",Type),
                new SqlParameter("@UserGI",user.UserGuid),Password,NeedPassword
            };
            await _sql.GetDataSetAsync("[PPR].[SP_GET_EXCEL_PASSWORD]", sqlParameters);
            var item = new ExcelPassword();
            item.Password = MakeSafe.ToSafeString(Password.Value.ToString());
            item.NeedPassword = MakeSafe.ToSafeBool(NeedPassword.Value);
            return item;

        }
        private ExcelPassword PopulateExcelPassword(DataSet ds)
        {
            var item = new ExcelPassword();

            if ((ds.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                item.Password = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["Password"]);
                item.NeedPassword = MakeSafe.ToSafeBool(ds.Tables[0].Rows[0]["NeedPassword"]);
                return item;
            }
            return null;
        }
        public async Task<ResponseEntity> ValidateExcelPassword(string Password, string ReportName, string Type, UserBase user)
        {
            SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
            SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

            IDbDataParameter[] sqlParameters =
          {
               new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid), new SqlParameter("@Type",Type),
                new SqlParameter("@Password",Password),  new SqlParameter("@ReportName",ReportName),
                ResponseKey,ResponseStatus
            };
            await _sql.ExecuteNonQueryAsync("[HRMS].[SP_VALIDATE_EXCEL_PASSWORD]", sqlParameters, CommandType.StoredProcedure);
            return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
        }

        #endregion
        public async Task<ResponseEntity> MRFRequest_Applicability(Guid EmployeeGI, UserBase user)
        {

            SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 250, Direction = ParameterDirection.Output };
            SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
            IDbDataParameter[] sqlParameters =
            {
            new SqlParameter("@JSONDATA",EmployeeGI),
            new SqlParameter("@UserGI",user.UserGuid),
            new SqlParameter("@CompanyGI",user.CompanyGuid),
            ResponseKey,ResponseStatus
            };
            await _sql.ExecuteNonQueryAsync("[HRMS].[SP_MRFREQUEST_VS_USER_APPLICABILITY]", sqlParameters, CommandType.StoredProcedure);
            return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
        }
        public async Task<UserRole> UserRole_Actions(UserBase User)
        {
            IDbDataParameter[] sqlparameters =
            {
             new SqlParameter("@CompanyGI",User.CompanyGuid),
            new SqlParameter("@UserGI",User.UserGuid)
            };
            var ds = await _sql.GetDataSetAsync("[HRMS].[SP_MASTER_ACTIONS]", sqlparameters);
            return PopulateUserRole_Actions(ds);

        }
        private UserRole PopulateUserRole_Actions(DataSet ds)
        {
            UserRole model = new UserRole();
            model.SalaryVisibility = new SalaryVisibility();

            if ((ds.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                model.View = MakeSafe.ToSafeBool(ds.Tables[0].Rows[0]["MasterView"]);
                model.SalaryVisibility.Code = MakeSafe.ToSafeInt32(ds.Tables[0].Rows[0]["ViewSalary"]);
                model.Create = MakeSafe.ToSafeBool(ds.Tables[0].Rows[0]["MasterCreate"]);
                model.Modify = MakeSafe.ToSafeBool(ds.Tables[0].Rows[0]["MasterEdit"]);
                model.Delete = MakeSafe.ToSafeBool(ds.Tables[0].Rows[0]["MasterDelete"]);
                model.Approve = MakeSafe.ToSafeBool(ds.Tables[0].Rows[0]["MasterApprove"]);
            }

            return model;
        }
        //public async Task<AttendanceStatus> GetAttendanceStatusType(UserBase User,int AttendanceStatusCode)
        //{
        //    IDbDataParameter[] sqlparameters =
        //    {
        //     new SqlParameter("@CompanyGI",User.CompanyGuid),
        //    new SqlParameter("@UserGI",User.UserGuid),
        //      new SqlParameter("@AttendanceStatusCode",AttendanceStatusCode)
        //    };
        //    var ds = await _sql.GetDataSetAsync("[HRMS].[SP_ATTENDANCE_STATUS_TYPE]", sqlparameters);
        //    return PopulateAttendanceStatusType(ds);

        //}
        //private AttendanceStatus PopulateAttendanceStatusType(DataSet ds)
        //{
        //    AttendanceStatus model = new AttendanceStatus();
        //    model.AttendanceStatusType = new AttendanceStatusType();

        //    if ((ds.Tables[0]?.Rows?.Count ?? 0) > 0)
        //    {
        //        model.AttendanceStatusType.Code = MakeSafe.ToSafeInt32(ds.Tables[0].Rows[0]["AttendanceStatusTypeCode"]);
        //        model.AttendanceStatusType.ShortName = MakeSafe.ToSafeString(ds.Tables[0].Rows[0]["Name"]);
               
        //    }

        //    return model;
        //}
        //public async Task<PreEnrollment> GetPFMaxLimit(UserBase user)
        //{
        //    IDbDataParameter[] sqlparameters =
        //   {
        //         new SqlParameter("@CompanyGI",user.CompanyGuid),


        //    };
        //    var ds = await _sql.GetDataSetAsync("[HRMS].[SP_GET_PF_MAX_LIMIT]", sqlparameters);
        //    return GetPFMaxValue(ds);
        //}
        //private PreEnrollment GetPFMaxValue(DataSet dataSet)
        //{
        //    PreEnrollment model = new PreEnrollment();

        //    if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
        //    {
        //        model.PFMaxValue = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["MaxValue"]);


        //    }
        //    return model;


        //}
        public async Task<ResponseEntity> GetSalaryPeriodLockDatewiseValidation(UserBase user,string SeletedDate)
        {

            SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 250, Direction = ParameterDirection.Output };
            SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
            IDbDataParameter[] sqlParameters =
            {
            new SqlParameter("@UserGI",user.UserGuid),
            new SqlParameter("@CompanyGI",user.CompanyGuid),
             new SqlParameter("@SELECTED_DATE",SeletedDate),
            ResponseKey,ResponseStatus
            };
            await _sql.ExecuteNonQueryAsync("[HRMS].[SP_SALARY_PERIOD_LOCK_DATEWISE_VALIDATION]", sqlParameters, CommandType.StoredProcedure);
            return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
        }
        public async Task<ResponseEntity> GetMasterInactiveValidation(string Master,int Code, UserBase user)
        {

            SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 250, Direction = ParameterDirection.Output };
            SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
            IDbDataParameter[] sqlParameters =
            {
            new SqlParameter("@master",Master),
             new SqlParameter("@CODE",Code),
            new SqlParameter("@UserGI",user.UserGuid),
            new SqlParameter("@CompanyGI",user.CompanyGuid),
            ResponseKey,ResponseStatus
            };
            await _sql.ExecuteNonQueryAsync("[HRMS].[SP_MASTER_INACTIVE_VALIDATION]", sqlParameters, CommandType.StoredProcedure);
            return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
        }
        public async Task<ResponseEntity<Int32>> GetAttendanceStatusCode(UserBase user,String LEAVETYPE)
        {
            SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
            SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };
            SqlParameter AttendanceStatusCode = new SqlParameter { ParameterName = "@AttendanceStatusCode", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            IDbDataParameter[] sqlParameters =
            {


                new SqlParameter("@UserGI", user.UserGuid),
                new SqlParameter("@CompanyGI", user.CompanyGuid),
                   new SqlParameter("@LEAVETYPE", LEAVETYPE),
                 ResponseKey,ResponseStatus,AttendanceStatusCode,
            };
            await _sql.ExecuteNonQueryAsync("[HRMS].[SP_GET_ATTENDANCE_STATUS_CODE]", sqlParameters, CommandType.StoredProcedure);
            return new ResponseEntity<Int32>(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value), Convert.ToInt32(AttendanceStatusCode.Value));
        }
    }
}

using PPR.Lite.Infrastructure.Helpers;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Repository.IRepository.General;
using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace PPR.Lite.Repository.Repository.General
{   
    public class DropdownRepository:IDropdownRepository
    {
        private ISqlConnectionProvider _sql;
        public DropdownRepository(ISqlConnectionProvider sqlConnectionProvider)
        {
            this._sql = sqlConnectionProvider;
        }
        public async Task<List<Dropdown>> GetJobRoleApplLocCombo(UserBase user)
        {
            try
            {
                IDbDataParameter[] sqlParameters =
              {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid)
            };
                var dataTable = await _sql.GetDataTableAsync("[HRMS].[SP_GET_LOCATION_COMBO]", sqlParameters, CommandType.StoredProcedure);
                return PopulateDropDownDetails(dataTable);
            
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
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

        public async Task<List<Dropdown>> GetDepartmentCombo(UserBase user)
        {
            try
            {
                IDbDataParameter[] sqlparameter =
                {
                    new SqlParameter("@CompanyGI",user.CompanyGuid),
                     new SqlParameter("@UserGI",user.UserGuid)

                };
                var dataTable = await _sql.GetDataTableAsync("[HRMS].[SP_GET_DEPARTMENT_COMBO]", sqlparameter, CommandType.StoredProcedure);
                return PopulateDropDownDetails(dataTable);
            }
            catch(Exception ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        public async Task<List<Dropdown>> GetGradeCombo(UserBase user)
        {
            try
            {
                IDbDataParameter[] sqlparameter =
                {
                    new SqlParameter("@CompnyGI",user.CompanyGuid),
                      new SqlParameter("@UserGI",user.UserGuid)
                };
                var dataTable = await _sql.GetDataTableAsync("[HRMS].[SP_GET_GRADE_COMBO]", sqlparameter, CommandType.StoredProcedure);
                return PopulateDropDownDetails(dataTable);
            }
            catch (Exception ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }

        }
        public async Task<List<Dropdown>> GetDesignationCombo(UserBase user)
        {
            try
            {
                IDbDataParameter[] sqlparameter =
                 {
                    new SqlParameter("@CompnyGI",user.CompanyGuid),
                    new SqlParameter("@UserGI",user.UserGuid)
                };
                var dataTable = await _sql.GetDataTableAsync("[HRMS].[SP_GET_DESIGNATION_COMBO]", sqlparameter, CommandType.StoredProcedure);
                return PopulateDropDownDetails(dataTable);
            }
            catch (Exception ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        public async Task<List<Dropdown>> GetCountryDropDown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
    {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid)
            };
            var dataTable = await _sql.GetDataTableAsync("[HRMS].[SP_COUNTRY_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        public async Task<List<Dropdown>> GetStateDropDown(int? CountryCode, UserBase user)
        {
            IDbDataParameter[] sqlParameters =
       {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),
                new SqlParameter("@CountryCode",CountryCode??null)
                };
            var dataTable = await _sql.GetDataTableAsync("[HRMS].[SP_STATE_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        public async Task<List<Dropdown>> GetDistrictDropdown(int? StateCode, UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                 {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),
                new SqlParameter("@StateCode",StateCode??null)
                };
            var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_DISTRICT_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        public async Task<List<Dropdown>> GetRegionDropdown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_REGION_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        public async Task<List<Dropdown>> GetLocationTypeDropdown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_LOCATION_TYPE_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        public async Task<List<Dropdown>> GetPTGroupDropDown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_PT_GROUP_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }
        public async Task<List<Dropdown>> GetESIGroupDropDown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_ESI_GROUP_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        public async Task<List<Dropdown>> GetInterviewLevelDropdown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_INTERVIEW_LEVEL_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }


        public async Task<List<Dropdown>> GetCalenderYearDropdown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[HRMS].[SP_CALENDAR_YEAR_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        public async Task<List<Dropdown>> GetFactorGroupDropDown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_FACTOR_GROUP_DROPDOWN]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        public async Task<List<Dropdown>> GetFactorTypeDropDown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_FACTOR_TYPE_DROPDOWN]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        public async Task<List<Dropdown>> GetUnitDropDown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_UNIT_DROPDOWN]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        public async Task<List<Dropdown>> GetLocationDropDown(UserBase user)
        {
            try
            {
                IDbDataParameter[] sqlParameters =
                 {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
                var dataTable = await _sql.GetDataTableAsync("[HRMS].[SP_LOCATION_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
                return PopulateDropDownDetails(dataTable);
            }
            catch(Exception ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);

            }
           
        }
        public async Task<List<Dropdown>> GetDesignationDropDown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[HRMS].[SP_DESIGNATION_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        public async Task<List<Dropdown>> GetDepartmentDropDown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[HRMS].[SP_DEPARTMENT_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        public async Task<List<Dropdown>> GetGradeDropDown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[HRMS].[SP_GRADE_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }

        public async Task<List<Dropdown>> GetPMSAppraisalAuthorityLevel(UserBase user)
        {
            try
            {
                IDbDataParameter[] sqlparameters =
            {
                 new SqlParameter("@CompanyGI",user.CompanyGuid),


            };
                var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_PPR_APPRAISAL_COMBO]", sqlparameters, CommandType.StoredProcedure);
                return PopulateDropDownDetails(dataTable);
            }

            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        public async Task<List<Dropdown>> GetGenderDropDown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
         {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid)
            };
            var dataTable = await _sql.GetDataTableAsync("[FIXED].[SP_GENDER_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);

        }
        public async Task<List<Dropdown>> GetEmployeeDropDown(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
            {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGuid",user.UserGuid),

                new SqlParameter("@GroupCompanyGI",user.GroupCompanyGI),


            };
            var datatable = await _sql.GetDataTableAsync("[PPR].[SP_GET_EMPLOYEE_SEARCH_DETAILS]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(datatable);
        }
        public async Task<List<Dropdown>> GetPMSAppraisalCombo(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_JOBROLE_DROPDOWN]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }
        public async Task<List<Dropdown>> GetPMSAppraisalRole(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_JOBROLE_DROPDOWN]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }
         public async Task<List<Dropdown>> GetReviewPeriod(UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),

                };
            var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_REVIEWPERIOD_DROPDOWN]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        } public async Task<List<Dropdown>> GetRatingTypeDropDown(int? FactorTypeCode,UserBase user)
        {
            IDbDataParameter[] sqlParameters =
                  {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),
                new SqlParameter("@FactorTyepCode",FactorTypeCode),

                };
            var dataTable = await _sql.GetDataTableAsync("[PPR].[SP_FACTORTYPE_COMBOFILL]", sqlParameters, CommandType.StoredProcedure);
            return PopulateDropDownDetails(dataTable);
        }
        


    }
}

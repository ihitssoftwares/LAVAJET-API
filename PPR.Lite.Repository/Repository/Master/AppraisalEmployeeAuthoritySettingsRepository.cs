using PPR.Lite.Infrastructure.Helpers;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Repository.IRepository.Master;
using PPR.Lite.Shared;
using PPR.Lite.Shared.General;
using PPR.Lite.Shared.PMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.Repository.Master
{
    public class AppraisalEmployeeAuthoritySettingsRepository: IAppraisalEmployeeAuthoritySettingsRepository
    {
        private readonly ISqlConnectionProvider _sql;
        public AppraisalEmployeeAuthoritySettingsRepository(ISqlConnectionProvider sql)
        {
            _sql = sql;
        }


        public async Task<PMS_Appraisal_Employee> GetEmployeeList(int JobRoleCode, int AuthorityLevelCode, UserBase User, Filter Filter)
        {
            try
            {
                IDbDataParameter[] sqlparameters =
                  {
                 new SqlParameter("@CompanyGI",User.CompanyGuid),
                 new SqlParameter("@UserGuid",User.UserGuid),
                 new SqlParameter("@GroupCompanyGI",User.GroupCompanyGI),
                 new SqlParameter("@JobRoleCode",JobRoleCode),
                 new SqlParameter("@AuthLevel",AuthorityLevelCode),

                 new SqlParameter("@JSONDATA",JsonHelper.ToJson(Filter))

            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_APPRAISAL_AUTHORITY_SETTINGS_LIST]", sqlparameters);
                return PopulateEmployeeList(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        private PMS_Appraisal_Employee PopulateEmployeeList(DataSet dataSet)
        {
            PMS_Appraisal_Employee model = new PMS_Appraisal_Employee();
            List<PMS_Appraisal_Employee> List = new List<PMS_Appraisal_Employee>();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var item = new PMS_Appraisal_Employee();
                    item.AppraisalRole = new MasterBase();
                    item.EmployeeGI = MakeSafe.ToSafeGuid(row["EmployeeGI"]);
                    item.EmployeeID = MakeSafe.ToSafeString(row["EmployeeID"]);
                    item.EmployeeName = MakeSafe.ToSafeString(row["FullName"]);
                    item.Designation = MakeSafe.ToSafeString(row["Designation"]);
                    item.Location = MakeSafe.ToSafeString(row["Location"]);
                    item.DOJ = MakeSafe.ToSafeDateTime(row["DOJ"]);
                    item.AppraisalRole.Name = MakeSafe.ToSafeString(row["AppraisalRole"]);
                    item.AuthLevelDetails = MakeSafe.ToSafeString(row["Authority"]);
                    item.Self = MakeSafe.ToSafeString(row["Self"]);
                    item.TotalRecords = MakeSafe.ToSafeInt32(row["TotalCount"]);
                    item.Active = MakeSafe.ToSafeInt32(row["ACTIVE"]);
                    List.Add(item);
                }
                model.PMS_Appraisal_EmployeeList = List;
            }

            return model;

        }

        public async Task<AppraisalEmployeeAuthoritySettings> GetEmployeeAuthoritySettingsDetails(Guid EmployeeGI, int JobRoleCode, UserBase User)
        {
            try
            {
                IDbDataParameter[] sqlparameters =
                  {
                 new SqlParameter("@CompanyGI",User.CompanyGuid),
                 new SqlParameter("@UserGuid",User.UserGuid),
                 new SqlParameter("@EmployeeGI",EmployeeGI),
                 new SqlParameter("@JobRoleCode",JobRoleCode)


            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_APPRAISAL_AUTHORITY_SETTINGS_DETAILS]", sqlparameters);
                return PopulateAuthorityDetails(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }


        private AppraisalEmployeeAuthoritySettings PopulateAuthorityDetails(DataSet dataSet)
        {
            AppraisalEmployeeAuthoritySettings model = new AppraisalEmployeeAuthoritySettings();
            model.AppraisalRole = new MasterBase();
            List<AuthoritySettings> AuthoritySettings = new List<AuthoritySettings>();
            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    model.AppraisalCode = MakeSafe.ToSafeInt32(row["AppraisalCode"]);
                    model.EmployeeGI = MakeSafe.ToSafeGuid(row["EmployeeGI"]);

                }
            }
            if ((dataSet.Tables[1]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    model.SelfLevel = MakeSafe.ToSafeBool(row["Self"]);
                    model.AppraisalRole.Code = MakeSafe.ToSafeInt32(row["JobRoleCode"]);
                }
            }

            if ((dataSet.Tables[2]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[2].Rows)
                {
                    var item = new AuthoritySettings();
                    item.AuthorityLevel = new AuthorityLevel();
                    item.EmployeeAuthority = new MasterBase();
                    item.AuthorityLevel.AuthorityLevelCode = MakeSafe.ToSafeInt32(row["AuthorityLevelCode"]);
                    item.AuthorityLevel.Name = MakeSafe.ToSafeString(row["AuthorityLevelName"]);
                    item.EmployeeAuthority.Code = MakeSafe.ToSafeInt32(row["ApproverEmpCode"]);
                    item.EmployeeAuthority.Name = MakeSafe.ToSafeString(row["Approver"]);
                    AuthoritySettings.Add(item);
                }
            }

            model.AuthoritySettingsList = AuthoritySettings;




            return model;

        }


        public async Task<ResponseEntity> Edit(AppraisalEmployeeAuthoritySettings model)
        {
            try
            {
                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {
            new SqlParameter("@JSONDATA",JsonHelper.ToJson(model)),
            ResponseKey,ResponseStatus
            };
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_APPRAISAL_AUTHORITY_SETTINGS_SAVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }

        }
        public async Task<ResponseEntity> BatchUpdate(AppraisalEmployeeAuthoritySettings model)
        {

            try
            {
                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {
                 new SqlParameter("@JSONDATA",JsonHelper.ToJson(model)),
                  new SqlParameter("@JSONFilter",JsonHelper.ToJson(model.Filter)),
            ResponseKey,ResponseStatus
            };
                await _sql.ExecuteNonQueryAsync("[PMS].[SP_APPRAISAL_AUTHORITY_SETTINGS_SAVE_BATCH_UPDATE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }

        }

    }
}

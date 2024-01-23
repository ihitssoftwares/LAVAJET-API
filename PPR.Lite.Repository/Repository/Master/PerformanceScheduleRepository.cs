using PPR.Lite.Infrastructure.Helpers;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Repository.IRepository.Master;
using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using PPR.Lite.Shared.PMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.Repository.Master
{
   public class PerformanceScheduleRepository: IPerformanceScheduleRepository
    {
        private readonly ISqlConnectionProvider _sql;
        public PerformanceScheduleRepository(ISqlConnectionProvider sql)
        {
            _sql = sql;
        }


        public async Task<PerformanceSchedule> GetList(UserBase user, int ReviewPeriodCode, int Schedule, Filter Filter)
        {

            try
            {
                IDbDataParameter[] sqlparameters =
                  {
                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                 new SqlParameter("@UserGuid",user.UserGuid),
                 new SqlParameter("@AppraisalCode",ReviewPeriodCode),
                 new SqlParameter("@Schedule",Schedule),
                 new SqlParameter("@GroupCompanyGI",user.GroupCompanyGI),

                 new SqlParameter("@JSONDATA",JsonHelper.ToJson(Filter))

            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_PERIODIC_PERFORMANCE_REVIEW_SCHEDULE_LIST]", sqlparameters);
                return PopulateAppraisalSheduleList(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        private PerformanceSchedule PopulateAppraisalSheduleList(DataSet dataSet)
        {
            PerformanceSchedule model = new PerformanceSchedule();
            List<PerformanceSchedule> List = new List<PerformanceSchedule>();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    DateTime? StartDate = null;
                    DateTime? EndDate = null;
                    var item = new PerformanceSchedule();
                    item.AppraisalRole = new MasterBase();
                    item.EmployeeGI = MakeSafe.ToSafeGuid(row["EmployeeGI"]);
                    item.EmployeeID = MakeSafe.ToSafeString(row["EmployeeID"]);
                    item.EmployeeCode = MakeSafe.ToSafeInt32(row["EmployeeCode"]);
                    item.EmployeeName = MakeSafe.ToSafeString(row["FullName"]);
                    item.Designation = MakeSafe.ToSafeString(row["Designation"]);
                    item.Department = MakeSafe.ToSafeString(row["Department"]);
                    item.Grade = MakeSafe.ToSafeString(row["Grade"]);
                    item.DOJ = MakeSafe.ToSafeDateTime(row["DOJ"]);
                    item.AppraisalRole.Name = MakeSafe.ToSafeString(row["Role"]);
                    item.Weightage = MakeSafe.ToSafeInt32(row["Weightage"]);
                    if (row["StartDate"].ToString() != "")
                        StartDate = MakeSafe.ToSafeDateTime(row["StartDate"]);
                    item.StartDate = StartDate;
                    if (row["EndDate"].ToString() != "")
                        EndDate = MakeSafe.ToSafeDateTime(row["EndDate"]);
                    item.EndDate = EndDate;
                    item.Comments = MakeSafe.ToSafeString(row["Comments"]);
                    item.CombinationCode = MakeSafe.ToSafeInt32(row["CombinationCode"]);
                    item.TotalRecords = MakeSafe.ToSafeInt32(row["TotalCount"]);
                    item.Active = MakeSafe.ToSafeInt32(row["ACTIVE"]);
                    List.Add(item);
                }
                model.PerformanceScheduleList = List;
            }

            return model;

        }

        public async Task<ResponseEntity> Save(PerformanceSchedule input)
        {
            try
            {

                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {
            new SqlParameter("@JSONDATA",JsonHelper.ToJson(input)),
            ResponseKey,ResponseStatus
            };
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_PERIODIC_PERFORMANCE_REVIEW_SCHEDULE_SAVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        public async Task<ResponseEntity> ResetAppraisal(UserBase user, int ReviewPeriodCode,  Guid EmployeeGI)
        {
            try
            {
                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGI",user.UserGuid),
                new SqlParameter("@AppraisalCode",ReviewPeriodCode),
                new SqlParameter("@EmployeeGI",EmployeeGI),
                ResponseKey,ResponseStatus
            };
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_PERIODIC_PERFORMANCE_REVIEW_SCHEDULE_RESET] ", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        public async Task<ResponseEntity> BatchUpdate(PerformanceSchedule input)
        {
            try
            {
                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {
                new SqlParameter("@JSONDATA",JsonHelper.ToJson(input)),
                new SqlParameter("@JSONDATA_Filter",JsonHelper.ToJson(input.Filter)),
                ResponseKey,ResponseStatus
            };
                await _sql.ExecuteNonQueryAsync("[PMS].[SP_APPRAISAL_SCHEDULE_SAVE_BATCH_UPDATE]", sqlParameters, CommandType.StoredProcedure);
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

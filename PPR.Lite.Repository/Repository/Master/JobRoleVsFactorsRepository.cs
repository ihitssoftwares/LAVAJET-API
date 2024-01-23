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
    public class JobRoleVsFactorsRepository: IJobRoleVsFactorsRepository
    {
        private readonly ISqlConnectionProvider _sql;
        public JobRoleVsFactorsRepository(ISqlConnectionProvider sql)
        {
            _sql = sql;
        }


        public async Task<RoleVsFactorShedule> typeList(UserBase user, int JobRoleCode)
        {

            try
            {

                IDbDataParameter[] sqlparameters =
                  {
                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                 new SqlParameter("@UserGuid",user.UserGuid),
                 new SqlParameter("@RoleCode",JobRoleCode),


            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_JOB_ROLE_VS_FACTOR_TYPE_LIST]", sqlparameters);

                return PopulateAppraisalSheduleList(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        private RoleVsFactorShedule PopulateAppraisalSheduleList(DataSet dataSet)
        {
            RoleVsFactorShedule model = new RoleVsFactorShedule();
            List<RoleVsFactorShedule> List = new List<RoleVsFactorShedule>();
            model.Appraisal = new PMS_Appraisal();
            model.AppraisalRole = new MasterBase();
            model.JobRoleVsFactorType = new JobRoleVsFactorType();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var item = new RoleVsFactorShedule();
                    item.JobRoleVsFactorType = new JobRoleVsFactorType();
                    item.JobRoleVsFactorType.FactorTypeCode = MakeSafe.ToSafeInt32(row["FactorTypeCode"]);
                    item.JobRoleVsFactorType.Name = MakeSafe.ToSafeString(row["FactorTypeName"]);
                    item.JobRoleVsFactorType.Applicability = MakeSafe.ToSafeBool(row["Applicability"]);
                    item.JobRoleVsFactorType.Weightage = MakeSafe.ToSafeInt32(row["Weightage"]);
                    List.Add(item);
                }
                model.TypeList = List;
            }

            return model;

        }
        public async Task<RoleVsFactorShedule> FactorList(UserBase user, int FactorTypeCode, int JobRole)
        {

            try
            {

                IDbDataParameter[] sqlparameters =
                  {
                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                 new SqlParameter("@UserGuid",user.UserGuid),
                 new SqlParameter("@FactorTypeCode",FactorTypeCode),
                 new SqlParameter("@JobRoleCode",JobRole),
               


            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_JOB_ROLE_VS_FACTOR_FACTORS_LIST]", sqlparameters);

                return PopulateAppraisalSheduleFactorsList(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        private RoleVsFactorShedule PopulateAppraisalSheduleFactorsList(DataSet dataSet)
        {
            RoleVsFactorShedule model = new RoleVsFactorShedule();
            List<RoleVsFactorShedule> List = new List<RoleVsFactorShedule>();
            model.Appraisal = new PMS_Appraisal();
            model.AppraisalRole = new MasterBase();
            model.JobRoleVsFactorType = new JobRoleVsFactorType();
            model.JobRoleVsFactorFactors = new JobRoleVsFactorFactors();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var item = new RoleVsFactorShedule();
                    item.JobRoleVsFactorFactors = new JobRoleVsFactorFactors();
                    item.JobRoleVsFactorFactors.FactorCode = MakeSafe.ToSafeInt32(row["FactorCode"]);
                    item.JobRoleVsFactorFactors.FactorName = MakeSafe.ToSafeString(row["FactorName"]);
                    item.JobRoleVsFactorFactors.FactorGroupCode = MakeSafe.ToSafeInt32(row["FactorGroupCode"]);
                    item.JobRoleVsFactorFactors.FactorGroupName = MakeSafe.ToSafeString(row["FactorGroupName"]);
                    item.JobRoleVsFactorFactors.UnitCode = MakeSafe.ToSafeInt32(row["UnitCode"]);
                    item.JobRoleVsFactorFactors.UnitName = MakeSafe.ToSafeString(row["UnitName"]);
                    item.JobRoleVsFactorFactors.Applicability = MakeSafe.ToSafeBool(row["Applicability"]);
                    item.JobRoleVsFactorFactors.Weightage = MakeSafe.ToSafeInt32(row["Weightage"]);
                    item.JobRoleVsFactorFactors.Target = MakeSafe.ToSafeInt32(row["Target"]);
                    item.JobRoleVsFactorFactors.RatingType = MakeSafe.ToSafeInt32(row["RatingTypeCode"]);
                    item.JobRoleGI = MakeSafe.ToSafeGuid(row["JobRoleGI"]);
                    List.Add(item);
                }
                model.FactorList = List;
            }

            return model;

        }
        public async Task<RoleVsFactorShedule> GetMapFactorList(UserBase user, int JobRole)
        {

            try
            {

                IDbDataParameter[] sqlparameters =
                  {
                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                 new SqlParameter("@UserGuid",user.UserGuid),
                 new SqlParameter("@JobRoleCode",JobRole),
                 


            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_JOB_MAP_ROLE_VS_FACTOR_FILL]", sqlparameters);

                return PopulateMapFactorList(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        private RoleVsFactorShedule PopulateMapFactorList(DataSet dataSet)
        {
            RoleVsFactorShedule model = new RoleVsFactorShedule();
            List<MapFactor> List = new List<MapFactor>();
            model.Appraisal = new PMS_Appraisal();
            model.AppraisalRole = new MasterBase();
            model.JobRoleVsFactorType = new JobRoleVsFactorType();
            model.JobRoleVsFactorFactors = new JobRoleVsFactorFactors();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var item = new MapFactor();
                   


                    item.FactorCode = MakeSafe.ToSafeInt32(row["FactorCode"]);
                    item.FactorName = MakeSafe.ToSafeString(row["FactorName"]);
                    item.FactorGroupCode = MakeSafe.ToSafeInt32(row["FactorGroupCode"]);
                    item.FactorGroupName = MakeSafe.ToSafeString(row["FactorGroupName"]);
                    item.FactorTypeName = MakeSafe.ToSafeString(row["FactorTypeName"]);
                    item.Applicability = MakeSafe.ToSafeBool(row["Applicability"]);
                  
                   
                    List.Add(item);
                }
                model.MapFactorList = List;
            }

            return model;

        }
        public async Task<ResponseEntity> Save(RoleVsFactorShedule input)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_JOB_ROLE_VS_FACTOR_FINAL_SAVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        public async Task<ResponseEntity> MapFactorListCreate(RoleVsFactorShedule input)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_JOB_ROLE_VS_FACTOR_SAVE]", sqlParameters, CommandType.StoredProcedure);
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

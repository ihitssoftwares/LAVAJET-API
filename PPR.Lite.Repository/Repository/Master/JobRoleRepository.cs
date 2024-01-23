using PPR.Lite.Infrastructure.Helpers;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Repository.IRepository.Master;
using PPR.Lite.Shared.DataBank.Generic.Master;
using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
namespace PPR.Lite.Repository.Repository.Master
{

   public class JobRoleRepository:IJobRoleRepository  
    {

        private readonly ISqlConnectionProvider _sql;
        public JobRoleRepository(ISqlConnectionProvider sql)
        {
            _sql = sql;
        }
        public async Task<JobRole> GetList(UserBase user, string TabIndex)
        {
            try
            {
                IDbDataParameter[] sqlparameter =
                {
                     new SqlParameter("@CompanyGI",user.CompanyGuid),
                     new SqlParameter("@UserGI",user.UserGuid),
                     new SqlParameter("Type",TabIndex)
                };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_JOB_ROLE_LIST]", sqlparameter);
                if (TabIndex == "APPROVAL_PENDING")
                    return CategoryPendingList(ds);
                else
                    return JobRoleList(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        private JobRole CategoryPendingList(DataSet dataset)
        {
            JobRole model = new JobRole();
            List<JobRole> List = new List<JobRole>();
            if ((dataset.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataset.Tables[0].Rows)
                {
                    model.ActiveCount = MakeSafe.ToSafeInt32(row["ActiveCount"]);
                    model.InActiveCount = MakeSafe.ToSafeInt32(row["InActiveCount"]);
                    model.PendingCount = MakeSafe.ToSafeInt32(row["PendingCount"]);
                }
            }
            if ((dataset.Tables[1]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataset.Tables[1].Rows)
                {
                    var item = new JobRole();
                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.Mode = MakeSafe.ToSafeString(row["EntryMode"]);
                    item.GI = MakeSafe.ToSafeGuid(row["JobRoleGI"]);
                    item.LogGI = MakeSafe.ToSafeGuid(row["LogGI"]);
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);
                    List.Add(item);
                }
                model.JobRoleList = List;
            }    
            return model;
        }
        private JobRole JobRoleList(DataSet dataSet)
        {
            JobRole model = new JobRole();
            //model.CategoryType = new CategoryType();
            List<JobRole> List = new List<JobRole>();
            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    model.ActiveCount = MakeSafe.ToSafeInt32(row["ActiveCount"]);
                    model.InActiveCount = MakeSafe.ToSafeInt32(row["InactiveCount"]);
                    model.PendingCount = MakeSafe.ToSafeInt32(row["PendingCount"]);

                }
            }

            if ((dataSet.Tables[1]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    var item = new JobRole();
                    //item.CategoryType = new CategoryType();
                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.GI = MakeSafe.ToSafeGuid(row["JobRoleGI"]);                   
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);

                    List.Add(item);
                }
                model.JobRoleList = List;
            }            
            return model;
        }

        public async Task<ResponseEntity> Save(JobRole input)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_JOBROLE_SAVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }

        }
        public async Task<ResponseEntity> Delete(UserBase user, JobRole model)
        {
            try
            {
                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {
                    new SqlParameter("@CompanyGI",user.CompanyGuid),
                    new SqlParameter("@UserGI",user.UserGuid),
                    new SqlParameter("@JobRoleGI",model.GI),
                    ResponseKey,ResponseStatus
                };
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_JOBROLE_DELETE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));

            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        public async Task<ResponseEntity> ApproveOrReject(JobRole model)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_JOBROLE_APPROVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }


        public async Task<JobRole> GetDetails(UserBase UserBase, Guid JobRoleGI, Guid LogGI, string Type)
        {
            try
            {

                IDbDataParameter[] sqlParameters =
           {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),

                new SqlParameter("@JobRoleGI",JobRoleGI),
               new SqlParameter("@LogGI",LogGI),
               new SqlParameter("@Type",Type),

            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_JOB_ROLE_DETAILS]", sqlParameters);
                return JobRole(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        private JobRole JobRole(DataSet dataSet)
        {
            int Tablecount = dataSet.Tables.Count;
            JobRole model = new JobRole();


            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {

                model.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Name"]);
                model.ShortName = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["ShortName"]);
                model.Active = MakeSafe.ToSafeBool(dataSet.Tables[0].Rows[0]["Active"]);
                model.Description = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Remarks"]);
                model.GI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["JobRoleGI"]);
                model.LogGI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["LogGI"]);



            }

            if ((dataSet.Tables[1]?.Rows?.Count ?? 0) > 0)
            {

                model.DepartmentCode = new int?[dataSet.Tables[1].Rows.Count];
                model.DepartmentName = new string[dataSet.Tables[1].Rows.Count];



                int i = 0;
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {

                    
                    model.DepartmentCode[i] = MakeSafe.ToSafeInt32(row["DepartmentCode"]);
                    model.DepartmentName[i] = MakeSafe.ToSafeString(row["DepartmentName"]);
                    i++;
                    

                }
            }
            if ((dataSet.Tables[2]?.Rows?.Count ?? 0) > 0)
            {

                model.LocationCode = new int?[dataSet.Tables[2].Rows.Count];
                model.LocationName = new string[dataSet.Tables[2].Rows.Count];


                int i = 0;
                foreach (DataRow row in dataSet.Tables[2].Rows)
                {


                    model.LocationCode[i] = MakeSafe.ToSafeInt32(row["LocationCode"]);
                    model.LocationName[i] =  MakeSafe.ToSafeString(row["LocationName"]);
                    i++;


                }
            }
            if ((dataSet.Tables[3]?.Rows?.Count ?? 0) > 0)
            {

                model.GradeCode = new int?[dataSet.Tables[3].Rows.Count];
                model.GradeName = new string[dataSet.Tables[3].Rows.Count];


                int i = 0;
                foreach (DataRow row in dataSet.Tables[3].Rows)
                {

                    model.GradeCode[i] = MakeSafe.ToSafeInt32(row["GradeCode"]);
                    model.GradeName[i]= MakeSafe.ToSafeString(row["GradeName"]);
                    //model.GradeCode[i] = MakeSafe.ToSafeInt32(dataSet.Tables[3].Rows[0]["GradeCode"]);
                    //model.GradeName[i] = MakeSafe.ToSafeString(dataSet.Tables[3].Rows[0]["GradeName"]);
                    i++;


                }
            }

            if ((dataSet.Tables[4]?.Rows?.Count ?? 0) > 0)
            {

                model.DesignationCode = new int?[dataSet.Tables[4].Rows.Count];
                model.DesignationName = new string[dataSet.Tables[4].Rows.Count];


                int i = 0;
                foreach (DataRow row in dataSet.Tables[4].Rows)
                {

                    model.DesignationCode[i]= MakeSafe.ToSafeInt32(row["DesignationCode"]);
                    model.DesignationName[i]= MakeSafe.ToSafeString(row["DesignationName"]);
                    //model.DesignationCode[i] = MakeSafe.ToSafeInt32(dataSet.Tables[4].Rows[0]["DesignationCode"]);
                    //model.DesignationName[i] = MakeSafe.ToSafeString(dataSet.Tables[4].Rows[0]["DesignationName"]);
                    i++;


                }
            }

            return model;
        }


    }
}

using ppr.Lite.Repository.IRepository.Master;
using PPR.Lite.Infrastructure.Helpers;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Repository.IRepository.Master;
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
    public class DesignationRepository : IDesignationRepository
    {
        private readonly ISqlConnectionProvider _sql;
        public DesignationRepository(ISqlConnectionProvider sql)
        {
            _sql = sql;
        }
        public async Task<Designation> GetList(UserBase user, string TabIndex)
        {

            try
            {

                IDbDataParameter[] sqlparameters =
                  {
                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                 new SqlParameter("@UserGuid",user.UserGuid),
                 new SqlParameter("@Type",TabIndex)

            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_DESIGNATION_LIST]", sqlparameters);

                if (TabIndex == "APPROVAL_PENDING")
                    return DesignationPendingList(ds);
                else
                    return DesignationList(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }



        private Designation DesignationList(DataSet dataSet)
        {
            Designation model = new Designation();
            List<Designation> List = new List<Designation>();
            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    model.ActiveCount = MakeSafe.ToSafeInt32(row["ActiveCount"]);
                    model.InactiveCount = MakeSafe.ToSafeInt32(row["InactiveCount"]);
                    model.PendingCount = MakeSafe.ToSafeInt32(row["PendingCount"]);

                }
            }

            if ((dataSet.Tables[1]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    var item = new Designation();

                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.GI = MakeSafe.ToSafeGuid(row["DesignationGI"]);
                    item.Description = MakeSafe.ToSafeString(row["Remarks"]);
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);
                    List.Add(item);
                }
                model.DesignationList = List;
            }

            return model;
        }

        private Designation DesignationPendingList(DataSet dataSet)
        {

            Designation model = new Designation();
            List<Designation> List = new List<Designation>();
            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    model.ActiveCount = MakeSafe.ToSafeInt32(row["ActiveCount"]);
                    model.InactiveCount = MakeSafe.ToSafeInt32(row["InactiveCount"]);
                    model.PendingCount = MakeSafe.ToSafeInt32(row["PendingCount"]);

                }
            }
            if ((dataSet.Tables[1]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    var item = new Designation();
                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.Mode = MakeSafe.ToSafeString(row["EntryMode"]);
                    item.Description = MakeSafe.ToSafeString(row["Remarks"]);
                    item.GI = MakeSafe.ToSafeGuid(row["DesignationGI"]);
                    item.LogGI = MakeSafe.ToSafeGuid(row["LogGI"]);
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);
                    List.Add(item);
                }
                model.DesignationList = List;

            }

            return model;

        }
        public async Task<ResponseEntity> Save(Designation Input)
        {
            try
            {

                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {
            new SqlParameter("@JSONDATA",JsonHelper.ToJson(Input)),
            ResponseKey,ResponseStatus
            };
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_DESIGNATION_SAVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }

        }

        public async Task<Designation> GetDesignationDetails(UserBase user, Guid DesignationGI, Guid LogGI, string type)
        {

            IDbDataParameter[] sqlParameters =
             {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
             
                new SqlParameter("@DesignationGI",DesignationGI),
                new SqlParameter("@LogGI",LogGI),
                new SqlParameter("@Type",type),

             };
            var ds = await _sql.GetDataSetAsync("[PPR].[SP_DESIGNATION_DETAILS]", sqlParameters);
            return GetDesignation(ds);

        }

        private Designation GetDesignation(DataSet dataSet)
        {
            int Tablecount = dataSet.Tables.Count;
            Designation model = new Designation();
            model.InterviewLevels = new MasterBase();
            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                model.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Name"]);
                model.ShortName = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["ShortName"]);
                model.Active = MakeSafe.ToSafeBool(dataSet.Tables[0].Rows[0]["Active"]);
                model.Description = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Remarks"]);
                model.GI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["DesignationGI"]);
                model.LogGI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["LogGI"]);
                model.MRFApproval = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["MRFApprovalRequired"]);
                model.InterviewLevels.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["InterviewLevels"]);
                model.InterviewLevels.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["LevelName"]);
                model.LicenseRequired = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["DrivingLicenseRequired"]);
                model.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["DesignationCode"]);
            }
                         
            return model;
        }

        public async Task<ResponseEntity> Delete(UserBase user, Designation model)
        {
            try
            {
                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {

                    new SqlParameter("@CompanyGI",user.CompanyGuid),
                    new SqlParameter("@UserGI",user.UserGuid),
                    new SqlParameter("@DesignationGI",model.GI),
                    ResponseKey,ResponseStatus
                };
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_DESIGNATION_DELETE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));

            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        public async Task<ResponseEntity> ApproveOrReject(Designation model)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_DESIGNATION_APPROVE]", sqlParameters, CommandType.StoredProcedure);
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
   


    


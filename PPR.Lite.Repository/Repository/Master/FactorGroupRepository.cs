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
    public class FactorGroupRepository : IFactorGroupRepository
    {
        private readonly ISqlConnectionProvider _sql;
        public FactorGroupRepository(ISqlConnectionProvider sql)
        {
            _sql = sql;
        }

        public async Task<FactorGroup> GetList(UserBase user, string TabIndex)
        {

            try
            {

                IDbDataParameter[] sqlparameters =
                  {
                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                 new SqlParameter("@UserGuid",user.UserGuid),
                 new SqlParameter("@Type",TabIndex)

            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_FACTORGROUP_LIST]", sqlparameters);

                if (TabIndex == "APPROVAL_PENDING")
                    return popupFactorGroupPendingList(ds);
                else
                    return popupFactorGroupList(ds);
            }
             catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }


        private FactorGroup popupFactorGroupList(DataSet dataSet)

        {
            FactorGroup model = new FactorGroup();
            List<FactorGroup> List = new List<FactorGroup>();
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
                    var item = new FactorGroup();

                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.GI = MakeSafe.ToSafeGuid(row["FactorGroupGI"]);
                    item.Description = MakeSafe.ToSafeString(row["Remarks"]);
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);
                    List.Add(item);
                }
                model.FactorGroupList = List;
            }

            return model;

        }

        private FactorGroup popupFactorGroupPendingList(DataSet dataSet)
        {

            FactorGroup model = new FactorGroup();
            List<FactorGroup> List = new List<FactorGroup>();
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
                    var item = new FactorGroup();
                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.Mode = MakeSafe.ToSafeString(row["EntryMode"]);
                    item.Description = MakeSafe.ToSafeString(row["Remarks"]);
                    item.GI = MakeSafe.ToSafeGuid(row["FactorGroupGI"]);
                    item.LogGI = MakeSafe.ToSafeGuid(row["LogGI"]);
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);
                    List.Add(item);
                }
                model.FactorGroupList = List;

            }

            return model;

        }

        public async Task<ResponseEntity> Save(FactorGroup input)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_FACTORGROUP_SAVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        public async Task<FactorGroup> GetDetails(UserBase UserBase, Guid FactorGroupGI, Guid LogGI, string Type)
        {
            try
            {

                IDbDataParameter[] sqlParameters =
           {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),

                new SqlParameter("@FactorGroupGI",FactorGroupGI),
               new SqlParameter("@LogGI",LogGI),
               new SqlParameter("@Type",Type),

            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_FACTORGROUP_DETAILS]", sqlParameters);
                return FactorGroup(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        private FactorGroup FactorGroup(DataSet dataSet)
        {
            int Tablecount = dataSet.Tables.Count;
            FactorGroup model = new FactorGroup();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                model.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Name"]);
                model.ShortName = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["ShortName"]);
                model.Active = MakeSafe.ToSafeBool(dataSet.Tables[0].Rows[0]["Active"]);
                model.Description = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Remarks"]);
                model.GI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["FactorGroupGI"]);
                model.LogGI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["LogGI"]);
                model.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["FactorGroupCode"]);
            }


            return model;
        }

        public async Task<ResponseEntity> Delete(UserBase user, FactorGroup model)
        {
            try
            {
                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {

                    new SqlParameter("@CompanyGI",user.CompanyGuid),
                    new SqlParameter("@UserGI",user.UserGuid),
                    new SqlParameter("@FactorGroupGI",model.GI),
                    ResponseKey,ResponseStatus
                };
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_FACTORGROUP_DELETE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));

            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        public async Task<ResponseEntity> ApproveOrReject(FactorGroup model)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_FACTORGROUP_APPROVE]", sqlParameters, CommandType.StoredProcedure);
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
    



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
   public class FactorRepository:IFactorRepository
    {
        private readonly ISqlConnectionProvider _sql;
        public FactorRepository(ISqlConnectionProvider sql)
        {
            _sql = sql;
        }



        public async Task<Factor> GetList(UserBase user, string TabIndex)
        {

            try
            {

                IDbDataParameter[] sqlparameters =
                  {
                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                 new SqlParameter("@UserGuid",user.UserGuid),
                 new SqlParameter("@Type",TabIndex)

            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_FACTOR_LIST]", sqlparameters);

                if (TabIndex == "APPROVAL_PENDING")
                    return FactorPendingList(ds);
                else
                    return FactorList(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }



        private Factor FactorList(DataSet dataSet)
        {
            Factor model = new Factor();
            List<Factor> List = new List<Factor>();
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
                    var item = new Factor();
                    item.FactorGroup = new FactorGroup();

                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.GI = MakeSafe.ToSafeGuid(row["FactorGI"]);
                    item.FactorGroup.Name = MakeSafe.ToSafeString(row["FactorGroup"]);
                    item.Description = MakeSafe.ToSafeString(row["Remarks"]);
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);
                    List.Add(item);
                }
                model.FactorList = List;
            }

            return model;

        }

        private Factor FactorPendingList(DataSet dataSet)
        {

            Factor model = new Factor();
            List<Factor> List = new List<Factor>();
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
                    var item = new Factor();
                    item.FactorGroup = new FactorGroup();
                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.Mode = MakeSafe.ToSafeString(row["EntryMode"]);
                    item.FactorGroup.Name = MakeSafe.ToSafeString(row["FactorGroup"]);              
                    item.Description = MakeSafe.ToSafeString(row["Remarks"]);
                    item.GI = MakeSafe.ToSafeGuid(row["FactorGI"]);
                    item.LogGI = MakeSafe.ToSafeGuid(row["LogGI"]);
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);
                    List.Add(item);
                }
                model.FactorList = List;

            }

            return model;

        }


        public async Task<ResponseEntity> Save(Factor input)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_FACTOR_SAVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        public async Task<Factor> GetDetails(UserBase UserBase, Guid FactorGI, Guid LogGI, string Type)
        {
            try
            {

                IDbDataParameter[] sqlParameters =
           {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),

                new SqlParameter("@FactorGI",FactorGI),
               new SqlParameter("@LogGI",LogGI),
               new SqlParameter("@Type",Type),

            };
            var ds = await _sql.GetDataSetAsync("[PPR].[SP_FACTOR_DETAILS]", sqlParameters);
            return Factor(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        private Factor Factor(DataSet dataSet)
        {
            int Tablecount = dataSet.Tables.Count;
            Factor model = new Factor();
            model.FactorGroup = new FactorGroup();
            model.FactorType = new FactorType();
            model.Unit = new Unit();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                model.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Name"]);
                model.ShortName = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["ShortName"]);
                model.Active = MakeSafe.ToSafeBool(dataSet.Tables[0].Rows[0]["Active"]);
                model.Description = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Remarks"]);
               
                model.FactorGroup.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["FactorGroupCode"]);
                model.FactorGroup.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["FactorGroup"]);
               
                model.FactorType.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["FactorTypeCode"]);
                model.FactorType.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["FactorName"]);

                model.Unit.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["UnitCode"]);
                model.Unit.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["UnitName"]);

                model.GI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["FactorGI"]);
                model.LogGI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["LogGI"]);
                model.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["Factorcode"]);
            }


            return model;
        }


        public async Task<ResponseEntity> Delete(UserBase user, Factor model)
        {
            try
            {
                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {

                    new SqlParameter("@CompanyGI",user.CompanyGuid),
                    new SqlParameter("@UserGI",user.UserGuid),
                    new SqlParameter("@FactorGI",model.GI),
                    ResponseKey,ResponseStatus
                };
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_FACTOR_DELETE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));

            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        public async Task<ResponseEntity> ApproveOrReject(Factor model)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_FACTOR_APPROVE]", sqlParameters, CommandType.StoredProcedure);
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

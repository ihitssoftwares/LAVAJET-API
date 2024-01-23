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
    public class RatingPaternRepository: IRatingPaternRepository
    {
        private readonly ISqlConnectionProvider _sql;
        public RatingPaternRepository(ISqlConnectionProvider sql)
        {
            _sql = sql;
        }

        public async Task<RatingPatern> GetList(UserBase user, string TabIndex)
        {

            try
            {

                IDbDataParameter[] sqlparameters =
                  {
                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                 new SqlParameter("@UserGuid",user.UserGuid),
                 new SqlParameter("@Type",TabIndex)

            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_RATING_PERIOD_LIST]", sqlparameters);

                if (TabIndex == "APPROVAL_PENDING")
                    return RatingPaternPendingList(ds);
                else
                    return RatingPaternList(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }



        private RatingPatern RatingPaternList(DataSet dataSet)
        {
            RatingPatern model = new RatingPatern();
            List<RatingPatern> List = new List<RatingPatern>();

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
                    var item = new RatingPatern();
                    item.FactorType = new FactorType();
                  

                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.GI = MakeSafe.ToSafeGuid(row["RatingTypeGI"]);
                 
                    item.FactorType.Name = MakeSafe.ToSafeString(row["FactorName"]);
                   // item.Toperiod = MakeSafe.ToSafeDateTime(row["PeriodTo"]);
                    item.Description = MakeSafe.ToSafeString(row["Remarks"]);
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);
                    List.Add(item);
                }
                model.RatingPatternList = List;
            }

            return model;

        }

        private RatingPatern RatingPaternPendingList(DataSet dataSet)
        {

            RatingPatern model = new RatingPatern();
            List<RatingPatern> List = new List<RatingPatern>();
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
                    var item = new RatingPatern();
                    item.FactorType = new FactorType();

                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.Mode = MakeSafe.ToSafeString(row["EntryMode"]);
                    item.FactorType.Code = MakeSafe.ToSafeInt32(row["FactorTypeCode"]);
                    item.FactorType.Name = MakeSafe.ToSafeString(row["FactorName"]);
                  
                    item.Description = MakeSafe.ToSafeString(row["Remarks"]);
                    item.GI = MakeSafe.ToSafeGuid(row["RatingPaternGI"]);
                    item.LogGI = MakeSafe.ToSafeGuid(row["LogGI"]);
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);
                    List.Add(item);
                }
                model.RatingPatternList = List;

            }

            return model;

        }


        public async Task<ResponseEntity> Save(RatingPatern input)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_RATING_PATTERN_SAVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        public async Task<RatingPatern> GetDetails(UserBase UserBase, Guid RatingPaternGI, Guid LogGI, string Type)
        {
            IDbDataParameter[] sqlParameters =
           {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),

                new SqlParameter("@RatingPaternGI",RatingPaternGI),
               new SqlParameter("@LogGI",LogGI),
               new SqlParameter("@Type",Type),

            };
            var ds = await _sql.GetDataSetAsync("[PPR].[SP_RATING_PATERN_DETAILS]", sqlParameters);
            return RevieWPeriod(ds);
        }


        private RatingPatern RevieWPeriod(DataSet dataSet)
        {
            int Tablecount = dataSet.Tables.Count;
            RatingPatern model = new RatingPatern();
            List<RatingSlab> List = new List<RatingSlab>();
            model.FactorType = new FactorType();
            model.RatingSlab = new List<RatingSlab>();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {

                model.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Name"]);
                model.ShortName = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["ShortName"]); 
                model.FactorType.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["FactorTypeCode"]); 

                model.FactorType.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["FactorName"]);
                model.Active = MakeSafe.ToSafeBool(dataSet.Tables[0].Rows[0]["Active"]);
                model.Description = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Remarks"]);

                model.GI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["RatingTypeGI"]);
                model.LogGI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["LogGI"]);
               


            }

            if ((dataSet.Tables[1]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    var item = new RatingSlab();
                    item.Rating= MakeSafe.ToSafeInt32(row["Rating"]);
                    item.Description = MakeSafe.ToSafeString(row["Description"]);
                    item.From =  MakeSafe.ToSafeInt32(row["From"]);
                    item.To = MakeSafe.ToSafeInt32(row["To"]);

                    model.RatingSlab.Add(item);

                }
            }


            return model;
        }

        public async Task<ResponseEntity> Delete(UserBase user, RatingPatern model)
        {
            try
            {
                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {

                    new SqlParameter("@CompanyGI",user.CompanyGuid),
                    new SqlParameter("@UserGI",user.UserGuid),
                    new SqlParameter("@RatingPaternGI",model.GI),
                    ResponseKey,ResponseStatus
                };
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_RATINT_PATTERN_DELETE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));

            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        public async Task<ResponseEntity> ApproveOrReject(RatingPatern model)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_RATING_PATTERN_APPROVE]", sqlParameters, CommandType.StoredProcedure);
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

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
   public  class ReviewPeriodRepository: IReviewPeriodRepository
    {
        private readonly ISqlConnectionProvider _sql;
        public ReviewPeriodRepository(ISqlConnectionProvider sql)
        {
            _sql = sql;
        }

        public async Task<ReviewPeriod> GetList(UserBase user, string TabIndex)
        {

            try
            {

                IDbDataParameter[] sqlparameters =
                  {
                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                 new SqlParameter("@UserGuid",user.UserGuid),
                 new SqlParameter("@Type",TabIndex)

            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_REVIEW_PERIOD_LIST]", sqlparameters);

                if (TabIndex == "APPROVAL_PENDING")
                    return ReviewPeriodPendingList(ds);
                else
                    return ReviewPeriodList(ds);
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }



        private ReviewPeriod ReviewPeriodList(DataSet dataSet)
        {
            ReviewPeriod model = new ReviewPeriod();
            List<ReviewPeriod> List = new List<ReviewPeriod>();
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
                    var item = new ReviewPeriod();
                    item.ReviewPeriodType = new ReviewPeriodType();

                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.GI = MakeSafe.ToSafeGuid(row["ReviewPeriodGI"]);
                    item.ReviewPeriodType.Name = MakeSafe.ToSafeString(row["CalendarYear"]);
                    item.Fromperiod = MakeSafe.ToSafeDateTime(row["PeriodFrom"]);
                    item.Toperiod = MakeSafe.ToSafeDateTime(row["PeriodTo"]);
                    item.Description = MakeSafe.ToSafeString(row["Remarks"]);
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);
                    List.Add(item);
                }
                model.ReviewPeriodList = List;
            }

            return model;

        }

        private ReviewPeriod ReviewPeriodPendingList(DataSet dataSet)
        {

            ReviewPeriod model = new ReviewPeriod();
            List<ReviewPeriod> List = new List<ReviewPeriod>();
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
                    var item = new ReviewPeriod();
                    item.ReviewPeriodType = new ReviewPeriodType();
                    item.Name = MakeSafe.ToSafeString(row["Name"]);
                    item.ShortName = MakeSafe.ToSafeString(row["ShortName"]);
                    item.Mode = MakeSafe.ToSafeString(row["EntryMode"]);
                    item.ReviewPeriodType.Name = MakeSafe.ToSafeString(row["CalendarYear"]);
                    item.Fromperiod = MakeSafe.ToSafeDateTime(row["PeriodFrom"]);
                    item.Toperiod = MakeSafe.ToSafeDateTime(row["PeriodTo"]);
                    item.Description = MakeSafe.ToSafeString(row["Remarks"]);
                    item.GI = MakeSafe.ToSafeGuid(row["ReviewPeriodGI"]);
                    item.LogGI = MakeSafe.ToSafeGuid(row["LogGI"]);
                    item.MakerChecker = MakeSafe.ToSafeBool(row["MakerChecker"]);
                    item.EditActive = MakeSafe.ToSafeBool(row["EditActive"]);
                    List.Add(item);
                }
                model.ReviewPeriodList = List;

            }

            return model;

        }
        public async Task<ResponseEntity> Save(ReviewPeriod input)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_REVIEW_PERIOD_SAVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        public async Task<ReviewPeriod> GetDetails(UserBase UserBase, Guid ReviewPeriodGI, Guid LogGI, string Type)
        {
            IDbDataParameter[] sqlParameters =
           {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),

                new SqlParameter("@ReviewPeriodGI",ReviewPeriodGI),
               new SqlParameter("@LogGI",LogGI),
               new SqlParameter("@Type",Type),

            };
           var ds = await _sql.GetDataSetAsync("[PPR].[SP_REVIEW_PERIOD_DETAILS]", sqlParameters);
            return RevieWPeriod(ds);
       }


        private ReviewPeriod RevieWPeriod(DataSet dataSet)
        {
            int Tablecount = dataSet.Tables.Count;
            ReviewPeriod model = new ReviewPeriod();
            model.ReviewPeriodType = new ReviewPeriodType();
            model.ReviewPeriodLevelList = new List<ReviewPeriodLevel>();
            List<ReviewPeriodLevel> ReviewPeriod_LevelList = new List<ReviewPeriodLevel>();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                model.Name = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Name"]);
                model.ShortName = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["ShortName"]);
                model.Active = MakeSafe.ToSafeBool(dataSet.Tables[0].Rows[0]["Active"]);
                model.Description = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Remarks"]);
                //model.Year = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["CalendarYear"]);
                model.ReviewPeriodType.Code= MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["CalendarYearCode"]);
                model.ReviewPeriodType.Name= MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["CalendarYear"]);

                model.Toperiod= MakeSafe.ToSafeDateTime(dataSet.Tables[0].Rows[0]["PeriodTo"]);
                model.Fromperiod= MakeSafe.ToSafeDateTime(dataSet.Tables[0].Rows[0]["PeriodFrom"]);

                model.GI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["ReviewPeriodGI"]);
                model.LogGI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["LogGI"]);
                model.Code = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["ReviewPeriodcode"]);
            }

            if((dataSet.Tables[1]?.Rows?.Count ?? 0) > 0)
              {
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {

                    ReviewPeriodLevel list = new ReviewPeriodLevel()
                    {

                        ReviewPeriodLevelCode = MakeSafe.ToSafeInt32(row["AuthorityLevelCode"]),
                        Name = MakeSafe.ToSafeString(row["Name"]),
                        EnableRating = MakeSafe.ToSafeBool(row["RatingLevel"]),
                        SelectRecommendatiobn = MakeSafe.ToSafeBool(row["RecomendationLevel"]),
                    };
                    ReviewPeriod_LevelList.Add(list);
                }
                model.ReviewPeriodLevelList = ReviewPeriod_LevelList;
            }
            return model;
        }

        public async Task<ResponseEntity> Delete(UserBase user, ReviewPeriod model)
        {
            try
            {
                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {

                    new SqlParameter("@CompanyGI",user.CompanyGuid),
                    new SqlParameter("@UserGI",user.UserGuid),
                    new SqlParameter("@ReviewPeriodGI",model.GI),
                    ResponseKey,ResponseStatus
                };
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_REVIEW_PERIOD_DELETE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));

            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }

        public async Task<ResponseEntity> ApproveOrReject(ReviewPeriod model)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_REVIEW_PERIOD_APPROVE]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }

        }

        public async Task<ReviewPeriod> GetLevel(UserBase UserBase)
        {
            IDbDataParameter[] sqlParameters =
              {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),
                new SqlParameter("@UserGI",UserBase.UserGuid),

             };
            var ds = await _sql.GetDataSetAsync("[PPR].[SP_GET_PPR_SHEDULE_CREATE_TYPE_AND_AUTHLEVEL]", sqlParameters);
            return GetLevel(ds);
        }
        private ReviewPeriod GetLevel(DataSet dataSet)
        {
            int Tablecount = dataSet.Tables.Count;
            ReviewPeriod model = new ReviewPeriod();
            model.ReviewPeriodLevelList = new List<ReviewPeriodLevel>();
           
            List<ReviewPeriodLevel> ReviewPeriod_Level = new List<ReviewPeriodLevel>();


            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {

                    ReviewPeriodLevel list = new ReviewPeriodLevel()
                    {
                        ReviewPeriodLevelCode = MakeSafe.ToSafeInt32(row["AuthorityLevelCode"]),
                        Name = MakeSafe.ToSafeString(row["Name"]),
                        EnableRating = MakeSafe.ToSafeBool(row["RatingLevel"]),
                        SelectRecommendatiobn = MakeSafe.ToSafeBool(row["RecomendationLevel"]),


                    };
                    ReviewPeriod_Level.Add(list);
                }
                model.ReviewPeriodLevelList = ReviewPeriod_Level;
            }
          
            return model;
        }
    }
}

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
    public class PPRSelfRepository: IPPRSelfRepository
    {
        private readonly ISqlConnectionProvider _sql;
        public PPRSelfRepository(ISqlConnectionProvider sql)
        {
            _sql = sql;
        }
        public async Task<PPRSelf> GetList(UserBase user)
        {

            try
            {

                IDbDataParameter[] sqlparameters =
                  {

                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGuid",user.UserGuid),

            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_ESSP_SELF_PPR_LIST]", sqlparameters);


                return PopulateSelfAppraisalList(ds);

            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        private PPRSelf PopulateSelfAppraisalList(DataSet dataSet)
        {
            PPRSelf model = new PPRSelf();

            List<PPRSelfList> PPRSelfList = new List<PPRSelfList>();
            model.PPRSelfList = new List<PPRSelfList>();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    PPRSelfList List = new PPRSelfList();
                    List.EmployeeCode = MakeSafe.ToSafeInt32(row["EmployeeCode"]);
                    List.ReviewPeriodCode = MakeSafe.ToSafeInt32(row["ReviewPeriodCode"]);
                    List.ReviewPeriodGI = MakeSafe.ToSafeGuid(row["ReviewPeriodGI"]);
                    List.ReviewPeriodName = MakeSafe.ToSafeString(row["ReviePeriodName"]);
                    List.StartDate = MakeSafe.ToSafeDateTime(row["StartDate"]);
                    List.JobRole = MakeSafe.ToSafeString(row["JobRole"]);
                    List.Status = MakeSafe.ToSafeString(row["Status"]);
                    List.TotalWeightage = MakeSafe.ToSafeDecimal(row["TotalWeightage"]);
                    List.wtgeClassification = MakeSafe.ToSafeString(row["wtgeClassification"]);
                    
                    PPRSelfList.Add(List);
                }
                model.PPRSelfList = PPRSelfList;
            }

            return model;

        }
        public async Task<ResponseEntity> ApprisalStart(UserBase user, int EmployeeCode, int AppraisalCode, int LevelCode, int CombinationCode)
        {
            try
            {

                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGuid",user.UserGuid),
                new SqlParameter("@EmployeeCode",EmployeeCode),
                new SqlParameter("@ReviewPeriodCode",AppraisalCode),
                new SqlParameter("@LevelCode",LevelCode),
                new SqlParameter("@CombinationCode",CombinationCode),
            ResponseKey,ResponseStatus
            };
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_ESSP_PPR_SELF_START]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }

        }
        public async Task<PPRSelf> GetAppraisalEmpDetails(UserBase UserBase, int EmployeeCode, int AppraisalCode, int CombinationCode)
        {
            IDbDataParameter[] sqlParameters =
              {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),
                new SqlParameter("@UserGuid",UserBase.UserGuid),
                new SqlParameter("@EmployeeCode",EmployeeCode),
                new SqlParameter("@ReviewPeriodCode",AppraisalCode),
                new SqlParameter("@CombinationCode",CombinationCode),

             };
            var ds = await _sql.GetDataSetAsync("[PPR].[SP_ESSP_PPR_SHEDULE_EMPLOYEE_DETAILS]", sqlParameters);
            return GetEmpDetails(ds);
        }
        private PPRSelf GetEmpDetails(DataSet dataSet)
        {
            int Tablecount = dataSet.Tables.Count;
            PPRSelf model = new PPRSelf();
            model.EmpDetailss = new EmpDetailss();



            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                model.EmpDetailss.EmployeeGI = MakeSafe.ToSafeGuid(dataSet.Tables[0].Rows[0]["EmployeeGI"]);
                model.EmpDetailss.EmployeeCode = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["empCode"]);
                model.EmpDetailss.EmployeeID = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["ID"]);
                model.EmpDetailss.EmployeeName = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["empName"]);
                model.EmpDetailss.DOJ = MakeSafe.ToSafeDateTime(dataSet.Tables[0].Rows[0]["DOJ"]);
               
                model.EmpDetailss.DepartmentName = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Department"]);
                model.EmpDetailss.DesignationName = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Designation"]);
                model.EmpDetailss.Location = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Location"]);
                model.EmpDetailss.Grade = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Grade"]);
               
                model.EmpDetailss.ReviewPeriodName = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["ReviewPeriodName"]);
                model.EmpDetailss.JobRoleRole = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["JobRole"]);
                model.EmpDetailss.HeadCode = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["HeadCode"]);
                model.EmpDetailss.ReportingManager = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["ReportingManager"]);
                model.EmpDetailss.EntryStatus = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["EntryStatus"]);
                model.EmpDetailss.AbsoluteURI = MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["AbsoluteUri"]);
            }


            return model;
        }
        public async Task<PPRSelf> GetTypeList(UserBase UserBase, int HeadCode, int level)
        {
            IDbDataParameter[] sqlParameters =
              {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),
                new SqlParameter("@UserGuid",UserBase.UserGuid),
                new SqlParameter("@HdCode",HeadCode),
                new SqlParameter("@level",level),

             };
            var ds = await _sql.GetDataSetAsync("[PPR].[SP_ESSP_PPR_SELF_TYPE_DETAILS]", sqlParameters);
            return PopulateTypeList(ds);
        }
        private PPRSelf PopulateTypeList(DataSet dataSet)
        {
            PPRSelf model = new PPRSelf();

            List<TypeList> TypeList = new List<TypeList>();
            model.TypeList = new List<TypeList>();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    TypeList List = new TypeList();
                    List.TypeCode = MakeSafe.ToSafeInt32(row["TypeCode"]);
                    List.TypeName = MakeSafe.ToSafeString(row["TypeName"]);
                    List.TypeWeightage = MakeSafe.ToSafeDecimal(row["TypeWeightage"]);
                    List.WeightageScore = MakeSafe.ToSafeDecimal(row["WeightageScore"]);
                    TypeList.Add(List);
                }
                model.TypeList = TypeList;
            }

            return model;

        }
        public async Task<PPRSelf> GetLevelList(UserBase UserBase, int HeadCode, int level)
        {
            IDbDataParameter[] sqlParameters =
              {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),
                new SqlParameter("@UserGuid",UserBase.UserGuid),
                new SqlParameter("@HdCode",HeadCode),
                new SqlParameter("@level",level),

             };
            var ds = await _sql.GetDataSetAsync("[PPR].[SP_ESSP_PPR_SELF_LEVEL_LIST]", sqlParameters);
            return PopulateLevelList(ds);
        }
        private PPRSelf PopulateLevelList(DataSet dataSet)
        {
            PPRSelf model = new PPRSelf();

            model.LevelList = new LevelList();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                model.LevelList.AuthorityLevelCode = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["AuthorityLevelCode"]);
                model.LevelList.Rating = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["Rating"]);
                model.LevelList.RecomendationLevel = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["RecomendationLevel"]);
                model.LevelList.TotalAuthorityLevel = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["TotalAuthorityLevel"]);
            }


            return model;

        }
        public async Task<PPRSelf> GetAppraisalCatList(UserBase UserBase, int HeadCode, int level, int ApprisalTypeCode)
        {
            IDbDataParameter[] sqlParameters =
              {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),
                new SqlParameter("@UserGuid",UserBase.UserGuid),
                new SqlParameter("@HeadCode",HeadCode),
                new SqlParameter("@FactorTypeCode",ApprisalTypeCode),
                new SqlParameter("@level",level),

             };
            var ds = await _sql.GetDataSetAsync("[PPR].[SP_ESSP_PPR_SELF_GET_SELF_FACTOR_LIST]", sqlParameters);
            return PopulateAppraisalCatList(ds);
        }
        private PPRSelf PopulateAppraisalCatList(DataSet dataSet)
        {
            PPRSelf model = new PPRSelf();

            List<AppraisalCategoryList> AppraisalCategoryList = new List<AppraisalCategoryList>();
            model.AppraisalCategoryList = new List<AppraisalCategoryList>();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    AppraisalCategoryList List = new AppraisalCategoryList();
                    List.HeadCode = MakeSafe.ToSafeInt32(row["HeadCode"]);
                    List.DtCode = MakeSafe.ToSafeInt32(row["DtCode"]);
                    List.FactorCode = MakeSafe.ToSafeInt32(row["FactCode"]);
                    List.FactorName = MakeSafe.ToSafeString(row["FactorName"]);
                    List.RatingMax = MakeSafe.ToSafeInt32(row["RatingMax"]);
                    List.Rating = MakeSafe.ToSafeInt32(row["Rating"]);
                    List.Remarks = MakeSafe.ToSafeString(row["Remarks"]);
                    List.Weightage = MakeSafe.ToSafeInt32(row["Weightage"]);
                    List.RatingApplicable = MakeSafe.ToSafeInt32(row["RatingApplicable"]);
                    List.CatgryName= MakeSafe.ToSafeString(row["GroupName"]);
                    List.Target = MakeSafe.ToSafeInt32(row["Target"]);
                    List.Achieved = MakeSafe.ToSafeInt32(row["Achieved"]);
                    AppraisalCategoryList.Add(List);
                }
                model.AppraisalCategoryList = AppraisalCategoryList;
            }

            return model;

        }
        public async Task<PPRSelf> GetAppraisalCommentList(UserBase UserBase, int HeadCode, int level)
        {
            IDbDataParameter[] sqlParameters =
              {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),
                new SqlParameter("@UserGuid",UserBase.UserGuid),
                new SqlParameter("@HeadCode",HeadCode),
                new SqlParameter("@level",level),

             };
            var ds = await _sql.GetDataSetAsync("[PPR].[SP_ESSP_SELF_GET_PPR_COMMENT_LIST]", sqlParameters);
            return PopulateAppraisalCommentList(ds);
        }
        private PPRSelf PopulateAppraisalCommentList(DataSet dataSet)
        {
            PPRSelf model = new PPRSelf();

            List<FactorComments> FactorComments = new List<FactorComments>();
            model.FactorComments = new List<FactorComments>();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    FactorComments List = new FactorComments();
                    List.LevelName = MakeSafe.ToSafeString(row["LevelName"]);
                    List.DtCode = MakeSafe.ToSafeInt32(row["DtCode"]);
                    List.HeadCode = MakeSafe.ToSafeInt32(row["HeadCode"]);
                    List.Comment = MakeSafe.ToSafeString(row["Comment"]);
                    List.ReviewRemarksShow = MakeSafe.ToSafeString(row["ReviewRemarksShow"]);
                    List.Editable = MakeSafe.ToSafeString(row["Editable"]);
                    FactorComments.Add(List);
                }
                model.FactorComments = FactorComments;
            }

            return model;

        }
        public async Task<PPRSelf> GetHistory(UserBase UserBase, int HeadCode, int FactorCode)
        {
            IDbDataParameter[] sqlParameters =
              {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),
                new SqlParameter("@UserGuid",UserBase.UserGuid),
                new SqlParameter("@HeadCode",HeadCode),
                new SqlParameter("@level",FactorCode),

             };
            var ds = await _sql.GetDataSetAsync("[PMS].[SP_ESSP_EMPLOYEE_HISTORY_DETAILS]", sqlParameters);
            return PopulateHistory(ds);
        }
        private PPRSelf PopulateHistory(DataSet dataSet)
        {
            PPRSelf model = new PPRSelf();

            List<HistoryDetails> HistoryDetails = new List<HistoryDetails>();
            model.HistoryDetails = new List<HistoryDetails>();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    HistoryDetails List = new HistoryDetails();
                    List.HdCode = MakeSafe.ToSafeInt32(row["LevelName"]);
                    List.FactorCode = MakeSafe.ToSafeInt32(row["DtCode"]);
                    List.FactorName = MakeSafe.ToSafeString(row["HeadCode"]);
                    List.LevelName = MakeSafe.ToSafeString(row["Comment"]);
                    List.FactorRating = MakeSafe.ToSafeInt32(row["ReviewRemarksShow"]);
                    List.LevelComment = MakeSafe.ToSafeString(row["Editable"]);
                    HistoryDetails.Add(List);
                }
                model.HistoryDetails = HistoryDetails;
            }

            return model;

        }
        public async Task<PPRSelf> CheckFinalSubmit(UserBase UserBase, int HeadCode, int level)
        {
            IDbDataParameter[] sqlParameters =
              {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),
                new SqlParameter("@UserGuid",UserBase.UserGuid),
                new SqlParameter("@HeadCode",HeadCode),
                new SqlParameter("@level",level),

             };
            var ds = await _sql.GetDataSetAsync("[PMS].[SP_ESSP_APPRAISAL_GET_FINALSUBMIT_STATUS]", sqlParameters);
            return PopulateCheckFinal(ds);
        }
        private PPRSelf PopulateCheckFinal(DataSet dataSet)
        {
            PPRSelf model = new PPRSelf();

            model.FinalSubmitt = new FinalSubmitt();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {

                model.FinalSubmitt.TotalCount = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["TotalCount"]);
                model.FinalSubmitt.Marked = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["Marked"]);
                model.FinalSubmitt.FSubmit = MakeSafe.ToSafeInt32(dataSet.Tables[0].Rows[0]["FSubmit"]);

            }

            return model;

        }

        public async Task<ResponseEntity> Save(PPRSelf input)
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
                await _sql.ExecuteNonQueryAsync("[PPR].[SP_ESSP_PPR_SAVE_SELF_DATA]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        public async Task<ResponseEntity> ValidateFinalSubmit(UserBase user, int LevelCode, int ApprCode, int EmpCodeFinalCheck)
        {
            try
            {

                SqlParameter ResponseKey = new SqlParameter { ParameterName = "@ResponseKey", SqlDbType = SqlDbType.VarChar, Size = 50, Direction = ParameterDirection.Output };
                SqlParameter ResponseStatus = new SqlParameter { ParameterName = "@ResponseStatus", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output };

                IDbDataParameter[] sqlParameters =
                {
                new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGuid",user.UserGuid),
                new SqlParameter("@LevelCode",LevelCode),
                new SqlParameter("@ApprCode",ApprCode),
                new SqlParameter("@EmpCodeFinalCheck",EmpCodeFinalCheck),
            ResponseKey,ResponseStatus
            };
                await _sql.ExecuteNonQueryAsync("[PMS].[SP_ESSP_EMP_FINAL_SUBMIT_CHECK]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }

        }

        public async Task<PPRSelf> GetApprovalList(UserBase user, int EmployeeCode)
        {

            try
            {

                IDbDataParameter[] sqlparameters =
                  {

                 new SqlParameter("@CompanyGI",user.CompanyGuid),
                new SqlParameter("@UserGuid",user.UserGuid),
               

            };
                var ds = await _sql.GetDataSetAsync("[PPR].[SP_ESSP_PPR_GET_APPROVAL_LIST]", sqlparameters);


                return PopulateApprovalList(ds);

            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        private PPRSelf PopulateApprovalList(DataSet dataSet)
        {
            PPRSelf model = new PPRSelf();

            List<PPRApprovalList> PPRApprovalList = new List<PPRApprovalList>();
            model.PPRApprovalList = new List<PPRApprovalList>();

            if ((dataSet.Tables[0]?.Rows?.Count ?? 0) > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    PPRApprovalList List = new PPRApprovalList();
                    List.EmployeeCode = MakeSafe.ToSafeInt32(row["EmployeeCode"]);
                    List.AppraisalCode = MakeSafe.ToSafeInt32(row["ReviewPeriodCode"]);
                    List.AuthorityLevelCode = MakeSafe.ToSafeInt32(row["AuthorityLevelCode"]);
                    List.Type = MakeSafe.ToSafeString(row["Type"]);
                    List.empID = MakeSafe.ToSafeString(row["empID"]);
                    List.empAName = MakeSafe.ToSafeString(row["empAName"]);
                    List.dptAName = MakeSafe.ToSafeString(row["dptAName"]);
                    List.dsgAName = MakeSafe.ToSafeString(row["dsgAName"]);
                    List.grdName = MakeSafe.ToSafeString(row["grdName"]);
                    List.ApprType = MakeSafe.ToSafeString(row["ApprType"]);
                    List.ApprStatus = MakeSafe.ToSafeString(row["ApprStatus"]);
                    List.empALocation = MakeSafe.ToSafeString(row["empALocation"]);
                    List.HedCode = MakeSafe.ToSafeInt32(row["HedCode"]);
                    List.RWOverallWtge = MakeSafe.ToSafeString(row["RWOverallWtge"]);
                   
                    PPRApprovalList.Add(List);
                }
                model.PPRApprovalList = PPRApprovalList;
            }

            return model;

        }
        public async Task<ResponseEntity> SaveReReviewDetails(PPRSelf input)
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
                await _sql.ExecuteNonQueryAsync("[PMS].[SP_ESSP_APPRAISAL_SAVE_RE_REVIEW_DETAILS]", sqlParameters, CommandType.StoredProcedure);
                return new ResponseEntity(ResponseKey.Value.ToString(), Convert.ToBoolean(ResponseStatus.Value));
            }
            catch (SqlException ex)
            {
                var ErrorMsg = ex.Message.ToString();
                throw new Exception(ErrorMsg, ex);
            }
        }
        public async Task<PPRSelf> GetFactorHistorySummary(UserBase UserBase, int EmployeeCode, int AppraisalCode, int CombinationCode)
        {
            IDbDataParameter[] sqlParameters =
              {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),
                new SqlParameter("@UserGuid",UserBase.UserGuid),
                new SqlParameter("@EmployeeCode",EmployeeCode),
                new SqlParameter("@AppraisalCode",AppraisalCode),
                new SqlParameter("@CombinationCode",CombinationCode),

             };
            var ds = await _sql.GetDataSetAsync("[PPR].[SP_ESSP_EMPLOYEE_PPR_FACTOR_HISTORY_SUMMARY]", sqlParameters);
            return GetFactorHistorySummary(ds);
        }
        private PPRSelf GetFactorHistorySummary(DataSet Ds)
        {
            int Tablecount = Ds.Tables.Count;
            PPRSelf List = new PPRSelf();
            List.Head = new List<String>();
            List.Rows = new List<HistoryRows>();


            if (Ds.Tables.Count > 0)
            {
                if (Ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataColumn col in Ds.Tables[0].Columns)
                    {
                        List.Head.Add(col.ColumnName);
                    }

                    foreach (DataRow Dr in Ds.Tables[0].Rows)
                    {
                        var Row = new HistoryRows();
                        Row.Columns = new List<string>();
                        foreach (DataColumn col in Ds.Tables[0].Columns)
                        {
                            Row.Columns.Add(Dr[col.ColumnName].ToString());
                        }
                        List.Rows.Add(Row);
                    }

                }

            }
            return List;
        }

        public async Task<string> GetRatingValue(UserBase UserBase, int Targetval, int AchieveValue, int TypeCode)
        {
            IDbDataParameter[] sqlParameters =
              {
                new SqlParameter("@CompanyGI",UserBase.CompanyGuid),
                new SqlParameter("@UserGuid",UserBase.UserGuid),
                new SqlParameter("@Targetval",Targetval),
                new SqlParameter("@AchieveValue",AchieveValue),
                new SqlParameter("@FactorTypeCode",TypeCode),

             };
             var ds = await _sql.GetDataSetAsync("[PPR].[SP_ESSP_PPR_SELF_RATING_VALUE_LIST]", sqlParameters);
            return GetPopulateRatingValue(ds);
        }
        private string GetPopulateRatingValue(DataSet dataSet)
        {
            PPRSelf model = new PPRSelf();

            string Rating= MakeSafe.ToSafeString(dataSet.Tables[0].Rows[0]["Rating"]);

            return Rating;

        }
    }
}

using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.Master
{
   public class PPRSelf:MasterBase
    {
        public List<PPRSelfList> PPRSelfList { get; set; }
        public EmpDetailss EmpDetailss { get; set; }
        public List<TypeList> TypeList { get; set; }
        public List<AppraisalCategoryList> AppraisalCategoryList { get; set; }
        public List<FactorComments> FactorComments { get; set; }
        public List<HistoryDetails> HistoryDetails { get; set; }
        public LevelList LevelList { get; set; }
        public List<PPRApprovalList> PPRApprovalList { get; set; }
        public FinalSubmitt FinalSubmitt { get; set; }
        public UserBase User { get; set; }
        public List<string> Head { get; set; }
        public List<HistoryRows> Rows { get; set; }

        public int EmployeeCode { get; set; }
        public int FinalSubmit { get; set; }
        public int LevelCode { get; set; }
        public int ApprCode { get; set; }
        public int RecommendationType { get; set; }
        public int IncrementPrctge { get; set; }
        public int ReviewerPercentage { get; set; }
        public int TotalTypeCount { get; set; }
        public int IsApprover { get; set; }
        public decimal TypeTotal { get; set; } = 0;
        public decimal TotalScore { get; set; }
        public int TypeCodeTemp { get; set; }
        public string TypName { get; set; }
        public int TotalRating { get; set; } = 0;
        public int EmpCodeFinalCheck { get; set; }
        public int CombinatinCode { get; set; }


    }

    public class HistoryRows
    {
        public List<string> Columns { get; set; }
    }

    public class PPRSelfList
    {
        public int EmployeeCode { get; set; }
        public int ReviewPeriodCode { get; set; }
        public Guid ReviewPeriodGI { get; set; }
        public string ReviewPeriodName { get; set; }
        public int FactorGroupCode { get; set; }
        public DateTime StartDate { get; set; }
        public int CompletedStatus { get; set; }
        public int ButtonEnable { get; set; }
        public decimal TotalWeightage { get; set; }
        public string Status { get; set; }
        public string JobRole { get; set; }
        public DateTime EndDate { get; set; }
        public string wtgeClassification { get; set; }
        public int CombinationCode { get; set; }


    }
    public class EmpDetailss
    {
        public Guid EmployeeGI { get; set; }
        public int EmployeeCode { get; set; }
        public string EmployeeID { get; set; }
        public string Category { get; set; }
        public string EmployeeName { get; set; }
        public string ReviewPeriodName { get; set; }
        public string JobRoleRole { get; set; }
        public string Location { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string Remarks { get; set; }
        public string catrgy { get; set; }
        public DateTime DOJ { get; set; }
        public string Grade { get; set; }
        public decimal Experience { get; set; }
        public int HeadCode { get; set; }
        public string ReportingManager { get; set; }
        public int EntryStatus { get; set; }
        public string AbsoluteURI { get; set; }
    }
    public class TypeList
    {
        public int TypeCode { get; set; }
        public string TypeName { get; set; }

        public decimal WeightageScore { get; set; }
        public decimal TypeWeightage { get; set; }


    }
    public class AppraisalCategoryList
    {
        public int DtCode { get; set; }
        public int MyProperty { get; set; }
        public int FactorCode { get; set; }
        public int Apprating { get; set; }
        public string ApprComments { get; set; }
        public int AppCode { get; set; }
        public int TypeCode { get; set; }
        public string CatgryName { get; set; }
        public string Remarks { get; set; }
        public string FactorName { get; set; }
        public int HideRatingCombo { get; set; }
        public int Rating { get; set; }
        public int RatingMax { get; set; }
        public int HeadCode { get; set; }
        public int Weightage { get; set; }
        public int RatingApplicable { get; set; }
        public decimal WeightedScore { get; set; } = 0;
        public int Target { get; set; }
        public int Achieved { get; set; }


    }
    public class FactorComments
    {
        public string LevelName { get; set; }
        public int DtCode { get; set; }
        public int HeadCode { get; set; }
        public string Comment { get; set; }

        public string ReviewRemarksShow { get; set; }
        public string Editable { get; set; }
    }
    public class HistoryDetails
    {
        public int HdCode { get; set; }
        public int FactorCode { get; set; }
        public string FactorName { get; set; }
        public string LevelName { get; set; }
        public int FactorRating { get; set; }
        public string LevelComment { get; set; }
    }

    public class FinalSubmitt
    {
        public int TotalCount { get; set; }
        public int Marked { get; set; }
        public int FSubmit { get; set; }
    }

    public class PPRApprovalList
    {
        public int EmployeeCode { get; set; }
        public string empID { get; set; }
        public bool bulkApr { get; set; }
        public string empAName { get; set; }
        public int TotalRecords { get; set; }
        public string dptAName { get; set; }
        public string dsgAName { get; set; }
        public int AppraisalCode { get; set; }
        public int AuthorityLevelCode { get; set; }
        public string Type { get; set; }
        public string grdName { get; set; }
        public string empRol { get; set; }
        public string RWOverallWtge { get; set; }
        public string ApprStatus { get; set; }
        public string empALocation { get; set; }
        public int HedCode { get; set; }
        public string ApprType { get; set; }
        public string wtgeClassification { get; set; }
        public int CombinationCode { get; set; }
    }

    public class LevelList
    {
        public int AuthorityLevelCode { get; set; }
        public int Rating { get; set; }

        public int RecomendationLevel { get; set; }
        public int TotalAuthorityLevel { get; set; }
    }
}


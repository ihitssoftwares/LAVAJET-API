using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.Master
{
    public interface IPPRSelfRepository
    {
        Task<PPRSelf> GetList(UserBase user);
        Task<PPRSelf> GetAppraisalEmpDetails(UserBase UserBase, int EmployeeCode, int AppraisalCode, int CombinationCode);
        Task<PPRSelf> GetTypeList(UserBase UserBase, int HeadCode, int level);
        Task<PPRSelf> GetLevelList(UserBase UserBase, int HeadCode, int level);
        Task<PPRSelf> GetAppraisalCatList(UserBase UserBase, int HeadCode, int level, int ApprisalTypeCode);
        Task<PPRSelf> GetAppraisalCommentList(UserBase UserBase, int HeadCode, int level);
        Task<PPRSelf> GetHistory(UserBase UserBase, int HeadCode, int FactorCode);
        Task<PPRSelf> CheckFinalSubmit(UserBase UserBase, int HeadCode, int level);
        Task<ResponseEntity> ValidateFinalSubmit(UserBase UserBase, int LevelCode, int ApprCode, int EmpCodeFinalCheck);
        Task<ResponseEntity> ApprisalStart(UserBase user, int EmployeeCode, int AppraisalCode, int LevelCode, int CombinationCode);
        Task<PPRSelf> GetFactorHistorySummary(UserBase user, int EmployeeCode, int AppraisalCode, int CombinationCode);
        Task<ResponseEntity> Save(PPRSelf input);

        Task<PPRSelf> GetApprovalList(UserBase user, int EmployeeCode);
        Task<ResponseEntity> SaveReReviewDetails(PPRSelf input);
        Task<string> GetRatingValue(UserBase user, int Targetval, int AchieveValue, int TypeCode);
    }
}

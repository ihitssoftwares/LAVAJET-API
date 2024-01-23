using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.Master
{
    public interface IReviewPeriodRepository
    {
        Task<ReviewPeriod> GetList(UserBase user, string TabIndex);
        Task<ResponseEntity> Save(ReviewPeriod input);
        Task<ReviewPeriod> GetDetails(UserBase UserBase, Guid ReviewPeriodGI, Guid LogGI, string Type);
        Task<ResponseEntity> Delete(UserBase user, ReviewPeriod model);
        Task<ResponseEntity> ApproveOrReject(ReviewPeriod model);
        Task<ReviewPeriod> GetLevel(UserBase UserBase);
    }
}

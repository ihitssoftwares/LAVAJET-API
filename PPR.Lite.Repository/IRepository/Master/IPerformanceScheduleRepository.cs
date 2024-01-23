using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using PPR.Lite.Shared.PMS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace PPR.Lite.Repository.IRepository.Master
{
   public interface IPerformanceScheduleRepository
    {
        Task<PerformanceSchedule> GetList(UserBase user, int ReviewPeriodCode, int Schedule, Filter Filter);
        Task<ResponseEntity> Save(PerformanceSchedule input);
        Task<ResponseEntity> ResetAppraisal(UserBase user, int ReviewPeriodCode, Guid EmployeeGI);

        Task<ResponseEntity> BatchUpdate(PerformanceSchedule input);
    }
}

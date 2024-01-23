using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.Master
{
    public interface IGradeRepository
    {
        Task<Grade> GetList(UserBase user, string TabIndex);
        Task<ResponseEntity> Save(Grade Input);
        Task<Grade> GetGradeDetails(UserBase UserBase, Guid GradeGI, Guid LogGI, string Type);
        Task<ResponseEntity> Delete(UserBase UserBase,Grade Input);
        Task<ResponseEntity> ApproveOrReject(Grade model);
    }
}

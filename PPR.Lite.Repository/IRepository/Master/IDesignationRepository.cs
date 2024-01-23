using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ppr.Lite.Repository.IRepository.Master
{
    public interface IDesignationRepository
    {
        Task<Designation> GetList(UserBase user, string TabIndex);
        Task<ResponseEntity> Save(Designation Input);
        Task<ResponseEntity> Delete(UserBase user, Designation model);
        Task<Designation> GetDesignationDetails(UserBase user, Guid DesignationGI, Guid LogGI, string type);
        Task<ResponseEntity> ApproveOrReject(Designation model);
    }

}

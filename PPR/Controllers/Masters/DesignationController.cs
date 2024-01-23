using PPR.Lite.Api.Helper;
using PPR.Lite.Repository.IRepository.Master;
using PPR.Lite.Shared.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ppr.Lite.Repository.IRepository.Master;

namespace PPR.Lite.Api.Controllers.Masters
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly IDesignationRepository _repo;
        public DesignationController(IDesignationRepository repo)
        {
            _repo = repo;
        }
        [HttpGet("List")]
        public async Task<IActionResult> GetList(string TabIndex)
        {
            return Ok(await _repo.GetList(UserHelper.GetUser(User), TabIndex));
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Designation Input)
        {
            Input.User = UserHelper.GetUser(User);
            return Ok(await _repo.Save(Input));
        }


        [HttpGet("GetDesignationDetails")]
        public async Task<IActionResult> GetDesignationDetails(Guid DesignationGI, Guid LogGI, string Type)
        {
            return Ok(await _repo.GetDesignationDetails(UserHelper.GetUser(User), DesignationGI, LogGI, Type));
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(Designation model)
        {
            //model.User = UserHelper.GetUser(User);
            return Ok(await _repo.Delete(UserHelper.GetUser(User), model));

        }
        [HttpPost("ApproveOrReject")]
        public async Task<IActionResult> ApproveOrReject(Designation model)
        {
            model.User = UserHelper.GetUser(User);
            return Ok(await _repo.ApproveOrReject(model));
        }

    }
}

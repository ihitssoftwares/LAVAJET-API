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

namespace PPR.Lite.Api.Controllers.Masters
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class JobRoleController : ControllerBase
    {
        private readonly IJobRoleRepository _repo;

        public JobRoleController(IJobRoleRepository repo)
        {
            _repo = repo;
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetList(string TabIndex)
        {
            return Ok(await _repo.GetList(UserHelper.GetUser(User), TabIndex));
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(JobRole input)
        {
            input.User = UserHelper.GetUser(User);
            return Ok(await _repo.Save(input));
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(JobRole model)
        {
            model.User = UserHelper.GetUser(User);
            return Ok(await _repo.Delete(UserHelper.GetUser(User), model));
        }
        [HttpPost("ApproveOrReject")]
        public async Task<IActionResult> ApproveOrReject(JobRole model)
        {
            model.User = UserHelper.GetUser(User);
            return Ok(await  _repo.ApproveOrReject(model));
        }

        [HttpGet("GetJobRoleDetails")]
        public async Task<IActionResult> GetJobRoleDetails(Guid JobRoleGI, Guid LogGI, string Type)
        {
            return Ok(await _repo.GetDetails(UserHelper.GetUser(User), JobRoleGI, LogGI, Type));
        }
    }
}

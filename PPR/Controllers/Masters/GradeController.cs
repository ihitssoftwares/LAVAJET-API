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

    public class GradeController : ControllerBase
    {
        private readonly IGradeRepository _repo;
        public GradeController(IGradeRepository repo)
        {
            _repo = repo;
        }
        [HttpGet("List")]
        public async Task<IActionResult> GetList(string TabIndex)
        {
            return Ok(await _repo.GetList(UserHelper.GetUser(User), TabIndex));
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Grade Input)
        {
            Input.User = UserHelper.GetUser(User);
            return Ok(await _repo.Save(Input));
        }
        [HttpGet("GetGradeDetails")]
        public async Task<IActionResult> GetGradeDetails(Guid GradeGI, Guid LogGI, string Type)
        {
            return Ok(await _repo.GetGradeDetails(UserHelper.GetUser(User), GradeGI, LogGI, Type));
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(Grade model)
        {
            //model.User = UserHelper.GetUser(User);
            return Ok(await _repo.Delete(UserHelper.GetUser(User), model));

        }
        [HttpPost("ApproveOrReject")]
        public async Task<IActionResult> ApproveOrReject(Grade model)
        {
            model.User = UserHelper.GetUser(User);
            return Ok(await _repo.ApproveOrReject(model));
        }
    }
}

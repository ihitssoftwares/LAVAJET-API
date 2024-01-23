using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PPR.Lite.Api.Helper;
using PPR.Lite.Repository.IRepository.Master;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPR.Lite.Api.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactorGroupController : Controller
    {
        private readonly IFactorGroupRepository _repo;

        public FactorGroupController(IFactorGroupRepository repo)
        {
            _repo = repo;
        }


        [HttpGet("List")]
        public async Task<IActionResult> GetList(string TabIndex)
        {
            return Ok(await _repo.GetList(UserHelper.GetUser(User), TabIndex));
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(FactorGroup input)
        {
            input.User = UserHelper.GetUser(User);
            return Ok(await _repo.Save(input));
        }


        [HttpGet("FactorGroupDetails")]
        public async Task<IActionResult> GetDetails(Guid FactorGroupGI, Guid LogGI, string Type)
        {
            return Ok(await _repo.GetDetails(UserHelper.GetUser(User), FactorGroupGI, LogGI, Type));
        }



        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(FactorGroup model)
        {
            model.User = UserHelper.GetUser(User);
            return Ok(await _repo.Delete(UserHelper.GetUser(User), model));

        }

        [HttpPost("ApproveOrReject")]
        public async Task<IActionResult> ApproveOrReject(FactorGroup model)
        {
            model.User = UserHelper.GetUser(User);
            return Ok(await _repo.ApproveOrReject(model));
        }


    }
}

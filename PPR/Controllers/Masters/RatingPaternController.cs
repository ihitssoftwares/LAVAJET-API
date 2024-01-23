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
    public class RatingPaternController : Controller
    {
        private readonly IRatingPaternRepository _repo;
        public RatingPaternController(IRatingPaternRepository repo)
        {
            _repo = repo;
        }
        [HttpGet("List")]
        public async Task<IActionResult> GetList(string TabIndex)
        {
            return Ok(await _repo.GetList(UserHelper.GetUser(User), TabIndex));
        }

        [HttpGet("tRatingPaternDetails")]
        public async Task<IActionResult> GetRatingPatternDetails(Guid RatingPaternGI, Guid LogGI, string Type)
        {
            return Ok(await _repo.GetDetails(UserHelper.GetUser(User), RatingPaternGI, LogGI, Type));
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(RatingPatern input)
        {
            input.User = UserHelper.GetUser(User);
            return Ok(await _repo.Save(input));
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(RatingPatern model)
        {
            model.User = UserHelper.GetUser(User);
            return Ok(await _repo.Delete(UserHelper.GetUser(User), model));

        }
        [HttpPost("ApproveOrReject")]
        public async Task<IActionResult> ApproveOrReject(RatingPatern model)
        {
            model.User = UserHelper.GetUser(User);
            return Ok(await _repo.ApproveOrReject(model));
        }


    }
}

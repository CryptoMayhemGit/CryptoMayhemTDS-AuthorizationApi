using Mayhem.Bl.Services.Interfaces;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Mayhem.TTDSAuthorizationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IAchievementService achievementService;

        public AchievementController(IAchievementService achievementService)
        {
            this.achievementService = achievementService;
        }

        [Route("GetLevel")]
        [HttpPost]
        [ProducesResponseType(typeof(GetLevelResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetLevel([FromBody] GetLevelRequest request)
        {
            GetLevelResponse getLevelResponse = await achievementService.GetLevelAsync(request);
            return CreatedAtAction(nameof(GetLevel), getLevelResponse);
        }
    }
}

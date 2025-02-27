using Microsoft.AspNetCore.Mvc;

namespace IotFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected int GetUserId()
        {
            var userIdClaim = User.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new Exception("User identifier is missing in the token.");
            }

            return int.Parse(userIdClaim);
        }
    }
}

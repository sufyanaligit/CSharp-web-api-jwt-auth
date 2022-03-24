using JstApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [Authorize]
        [HttpGet("Admins")]
        public IActionResult Admins()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.Username}");
        }
        [HttpGet]
        public IActionResult Public()
        {
            return Ok("You are in public");
        }
        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if(identity != null)
            {
                var userClaims = identity.Claims;

                return new UserModel 
                {
                    Username = userClaims.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    GivenName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value,
                };
            }
            return null;
        }
    }
}

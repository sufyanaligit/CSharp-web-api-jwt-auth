using JstApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        public LoginController (IConfiguration config)
        {
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);

            if(user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }
            return NotFound();
        }

        private UserModel Authenticate(UserLogin userLogin)
        {
            var currentUser = UserConstants.Users.FirstOrDefault(x => x.Username.ToLower() == userLogin.Username.ToLower() &&
            x.Password.ToLower() == userLogin.Password.ToLower());

            if (currentUser != null)
                return currentUser;

            return null;
        }

        private string Generate(UserModel userModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userModel.Username),
                new Claim(ClaimTypes.Email, userModel.EmailAddress),
                new Claim(ClaimTypes.GivenName, userModel.GivenName),
                new Claim(ClaimTypes.Surname, userModel.Surname),
                new Claim(ClaimTypes.Role, userModel.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
                
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

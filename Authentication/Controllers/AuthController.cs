using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Authentication.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
            _logger.LogInformation("AuthController constructor hit 2!");
        }
        [HttpPost("Authenticate")]

        public IActionResult Authenticate([FromBody] User user)
        {
            if (user.Username == "admin" || user.Password == "password")
            {
                IList<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, "admin@gmail.com"),
                    new Claim("Department", "HR")


                };

                var explationDate = DateTime.UtcNow.AddMinutes(10);

                return Ok(new
                {
                    access_token = CreateToken(claims, explationDate),
                    expires_at = explationDate
                });
            }

            ModelState.AddModelError("Unauthorized", "Invalid username or password");
            return new OkObjectResult(true);
        }


        private string CreateToken(IEnumerable<Claim> claims, DateTime expireAt)
        {
            var jwt = new JwtSecurityToken(
                
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expireAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a1f4e3c9d5b60789c3e2f1a0b4d6c8e7f9a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5\r\n")),
                    SecurityAlgorithms.HmacSha256Signature
                )

            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}

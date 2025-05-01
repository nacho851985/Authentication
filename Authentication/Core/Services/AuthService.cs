using Amazon.Lambda.APIGatewayEvents;
using Authentication.Core.Interfaces;
using Authentication.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Core.Services
{
    public class AuthService : IAuthService
    {
        public Token Authenticate(User user)
        {

            IList<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.isAdmin.ToString() ?? "false")
                };

            var explationDate = DateTime.UtcNow.AddMinutes(10);

            Token token = new Token();

            return token.BuildToken(claims); ;
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

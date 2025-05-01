using Amazon.Lambda.APIGatewayEvents;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Authentication.Core.Models
{
    public class Token
    {
        public string access_token { get; set; }
        public DateTime expires_at { get; set; }

        public APIGatewayProxyResponse BuildToken(IList<Claim> claims)
        {
            var explationDate = DateTime.UtcNow.AddMinutes(10);

            Token token = new Token
            {
                access_token = CreateToken(claims, explationDate),
                expires_at = explationDate
            };

            return BuildCorsResponse(220, JsonSerializer.Serialize(new { token }));
        }

        private APIGatewayProxyResponse BuildCorsResponse(int statusCode, string body)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Headers = new Dictionary<string, string>
                {
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Headers", "Content-Type, Authorization" },
                    { "Access-Control-Allow-Methods", "POST, GET, OPTIONS" }
                },
                Body = body,
            };
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

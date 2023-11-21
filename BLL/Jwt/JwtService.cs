using DAL.Models;
using DTO.Repository;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly string secret = "this is my custom Secret key for authentication";


        public string CreateToken(CustomerDTO customer)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("email", customer.CustEmail),
                new Claim("id", customer.CustId + ""),
                new Claim("password", customer.CustPassword),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signingCredentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public int GetIdFromToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);

            var userIdClaim = token.Claims.First(c => c.Type == "id");

            return Convert.ToInt32(userIdClaim.Value);
        }

        public string GetPasswordFromToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);

            var userIdClaim = token.Claims.First(c => c.Type == "password");

            return userIdClaim.Value;
        }

        public string GetEmailFromToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);

            var userIdClaim = token.Claims.First(c => c.Type == "email");

            return userIdClaim.Value;
        }
    }
}

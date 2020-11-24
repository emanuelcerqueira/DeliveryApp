using DeliveryApp.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using DeliveryApp.Util;
using DeliveryApp.Services.Models;

namespace DeliveryApp.Services 
{
    public interface ITokenService {
        Token GenerateToken(User user);
    }
    public class TokenService : ITokenService
    {
        public Token GenerateToken(User user)
        {
        var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Constants.JWT_SECRET);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = tokenHandler.WriteToken(token);

            return new Token(jwtToken, user, tokenDescriptor.Expires.Value);
        }
    }
}
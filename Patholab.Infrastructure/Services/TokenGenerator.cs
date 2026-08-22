using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Patholab.Application.Interfaces;

namespace Patholab.Infrastructure.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(int userId, string username, string role, List<string> permissions, out DateTime expiration)
        {
            var secret = _configuration["JwtSettings:Secret"] ?? "SuperSecretKeyPatholab1234567890Patholab";
            var issuer = _configuration["JwtSettings:Issuer"] ?? "PatholabAPI";
            var audience = _configuration["JwtSettings:Audience"] ?? "PatholabMVC";
            var expiryMinutes = double.Parse(_configuration["JwtSettings:ExpiryMinutes"] ?? "120");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            expiration = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permission", permission));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

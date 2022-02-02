using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameCatalog.Entity.Models;
using GameCatalog.Services.Interfaces;
using GameCatalogv2.Entity.Enum;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GameCatalog.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string Generate(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] secret = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
            string audience = _configuration["JWT:Audience"];
            string issuer = _configuration["JWT:Issuer"];
            int expirationTime = Convert.ToInt32(_configuration["JWT:ExpirationTime"]);

            ClaimsIdentity claimsIdentities = new(
                new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.FullName.ToString()),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString())
                }
            );

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(secret);
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor();

            tokenDescriptor.Subject = claimsIdentities;
            tokenDescriptor.Issuer = issuer;
            tokenDescriptor.Audience = audience;
            tokenDescriptor.Expires = DateTime.UtcNow.AddHours(expirationTime);
            tokenDescriptor.SigningCredentials = signingCredentials;

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
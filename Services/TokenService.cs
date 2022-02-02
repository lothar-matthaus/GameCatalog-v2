using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameCatalog.Entity.Enum;
using GameCatalog.Entity.Models;
using GameCatalog.Services.Interfaces;
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
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role, Enum.GetName(typeof(UserRole), user.UserRole)),
                    new Claim(ClaimTypes.Email, user.Email),
                }
            );

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(secret);
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

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
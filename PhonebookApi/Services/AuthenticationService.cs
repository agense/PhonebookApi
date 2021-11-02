using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PhonebookApi.Configurations;
using PhonebookBusiness.Interfaces;
using PhonebookBusiness.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PhonebookApi.Services
{
    public class AuthenticationService : IAuthentication {

        private readonly JwtConfig _jwtConfig;

        public AuthenticationService(IOptionsMonitor<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig.CurrentValue;
        }

        public string Authenticate(ApplicationUser user) {
            var token = GenerateToken(user);
            return token;
        }


        private string GenerateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(_jwtConfig.ExpiresIn),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}

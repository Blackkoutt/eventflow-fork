using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventFlowAPI.Logic.Identity.Services.Services
{
    public class JWTGeneratorService : IJWTGeneratorService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;

        public JWTGeneratorService(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JWTAuth");
        }

        public string GenerateToken(User user, IList<string>? roles) 
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaimsForUser(user, roles);
            var tokenOptions = GetTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }


        private SigningCredentials GetSigningCredentials() 
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]!);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private List<Claim> GetClaimsForUser(User user, IList<string>? roles) 
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id!),
                new Claim("name", user.Name!),
                new Claim("surname", user.Surname!),
                new Claim("emailAddress", user.Email!),
                new Claim("dateOfBirth", user.DateOfBirth.ToString(DateFormat.Date)!)
            };
            if(roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim("userRoles", role));
                }
            }
           
            return claims;
        }

        private JwtSecurityToken GetTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            return new JwtSecurityToken(
                issuer: _jwtSettings["validIssuer"],
                audience: _jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
                signingCredentials: signingCredentials
            );
        }

        
    }
}

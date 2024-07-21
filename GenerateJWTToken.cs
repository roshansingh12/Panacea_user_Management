//using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Panacea_User_Management.Models;

namespace Panacea_User_Management
{
    public class GenerateJWTToken
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User_Model> _userManager;
        private readonly ILogger _logger;
        public GenerateJWTToken(IConfiguration configuration, UserManager<User_Model> userManager,ILogger logger)
        {
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<string> JWTToken(User_Model _user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            _logger.LogInformation("line 1");
            /*var  key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
            var tokenDescrpitor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, UserId)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescrpitor);*/
            var UserRoles = await _userManager.GetRolesAsync(_user);
            _logger.LogInformation("line 2");
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,_user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            _logger.LogInformation("line 3");
            foreach (var role in UserRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            _logger.LogInformation("line 4");
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _logger.LogInformation("line 5");
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            _logger.LogInformation($"6 {new JwtSecurityTokenHandler().WriteToken(token)}");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

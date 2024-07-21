using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panacea_User_Management.Models;
using Panacea_User_Management.Panacea_DbContext;
using Panacea_User_Management.Services;
using System.Collections;

namespace Panacea_User_Management.Container
{
    public class Login:ILogin
    {
        //private readonly HttpContext _httpContext;
        //private readonly Panacea_User_Management_DbContext _context;
        private readonly UserManager<User_Model> _userManager;
        private readonly IConfiguration _configs;
        private readonly ILogger<Login> _logger;
        public Login(UserManager<User_Model> userManager, IConfiguration configs, ILogger<Login> logger)//,HttpContext httpContext)
        {
            _configs = configs;
            _userManager = userManager;
            _logger = logger;
            //_httpContext = httpContext;
        }
        public async Task<ActionResult> _Login(UserDTO user,HttpContext _httpContext)
        {
            var _user = await _userManager.FindByIdAsync(user.UserName);

            _logger.LogInformation(_user.NormalizedUserName);
            if(_user!=null && await _userManager.CheckPasswordAsync(_user, user.Password))
            {
                _logger.LogInformation("matched , now we logging in!");
                var get_token = new GenerateJWTToken(_configs,_userManager,_logger);
                var token = await get_token.JWTToken(_user);
                _logger.LogInformation($"Here : {token}");
                var CookieConfigs = new CookieOptions
                {
                    HttpOnly=true,
                    Secure=true,
                    SameSite=SameSiteMode.Strict
                };
                _httpContext.Response.Cookies.Append("jwt",token,CookieConfigs);
                return new OkResult();
                //_httpContext.Response.Cookies.Append("jwt", token, CookieConfigs);
            }
            _logger.LogInformation($"{user.Password} \n {_user.PasswordHash} \n {await _userManager.CheckPasswordAsync(_user, user.Password)}");
            return new BadRequestResult();
        }
    }
}

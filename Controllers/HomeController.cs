using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Panacea_User_Management.Models;
using Panacea_User_Management.Panacea_DbContext;
using Panacea_User_Management.Services;
using System.Diagnostics;
using System.Linq;

namespace Panacea_User_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRegister _register;
        private readonly ILogin _login;
        private readonly ILogger<HomeController> _logger;
        private readonly Panacea_User_Management_DbContext _context;
        private readonly UserManager<User_Model> _userManager;
        public HomeController(UserManager<User_Model> userManager,ILogin login,ILogger<HomeController> logger, Panacea_User_Management_DbContext context, IRegister register)
        {
            _login = login;
            _register = register;
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        //User -> register
        [HttpPost]
        public async Task<ActionResult> Register(Panacea_User User)
        {
            /*_logger.LogInformation("user creation started!");
            var _user = _context.P_Users.FirstOrDefault(user => user.Username == User.Username);
            if (_user != null)
            {
                return RedirectToAction("Error");
            }
            else
            {
                await _context.P_Users.AddAsync(User);
                await _context.SaveChangesAsync();
            }
            _logger.LogInformation("user created successfully!");
            return RedirectToAction("Login");*/
            try
            {
                await _register.Register(User);
                return RedirectToAction("Login");
            }
            catch(Exception Ex)
            {
                _logger.LogError($"Exception occurred : {Ex}");
                return RedirectToAction("Error");
            }
            
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (Request.Cookies.TryGetValue("jwt", out var value))
            {
                _logger.LogInformation(value);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserDTO user)
        {
            try
            {
                var response =await _login._Login(user,HttpContext);
                if(response.ToString()== "Microsoft.AspNetCore.Mvc.OkResult")
                {
                    var isadmin = User.IsInRole("Admin");
                    if (isadmin)
                    {
                        return RedirectToAction("Dashboard","Admin");
                    }
                    else
                    {
                        return RedirectToAction("LoggedIn");
                    }
                }
                else
                {
                    return RedirectToAction("Login");
                }//RedirectToAction("LoggedIn");
            }
            catch (Exception Ex)
            {
                _logger.LogError($"Exception occurred : {Ex}");
                return RedirectToAction("Error");
            }
        }
        public IActionResult LoggedIn()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public override bool Equals(object? obj)
        {
            return obj is HomeController controller &&
                   EqualityComparer<ILogger<HomeController>>.Default.Equals(_logger, controller._logger);
        }
        [HttpPost]
        public async Task<IActionResult> Log_out()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Index", "Home");
        }
    }
}
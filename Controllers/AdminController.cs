using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panacea_User_Management.Panacea_DbContext;
using Panacea_User_Management.Models;
using System.Security.Permissions;
using Microsoft.AspNetCore.Identity;

namespace Panacea_User_Management.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly Panacea_User_Management_DbContext _context;
        private readonly ILogger<AdminController> _logger;
        public AdminController(Panacea_User_Management_DbContext context, ILogger<AdminController> logger) {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Dashboard()
        {
            var total_users = await _context.P_Users.ToListAsync();
            var Active_count = await _context.P_Users.Where(user => user.IsActive).ToListAsync();
            TempData["user_count"] = total_users.Count();
            TempData["Active_Users"] = Active_count.Count();
            TempData["InActive_Users"] = total_users.Count() - Active_count.Count();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> User_List()
        {

            var users = await _context.P_Users.Where(u=>u.Username.Contains("Admin")==false).ToListAsync();
            ViewBag.users = users;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> User_List(string searchString, string sortingstring)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["SortFilter"] = sortingstring;
            var users = await _context.P_Users.Where(u => u.Username.Contains("Admin") == false).ToListAsync();
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(user =>
                user.FirstName.Contains(searchString) ||
                user.LastName.Contains(searchString) ||
                user.MiddleName.Contains(searchString) ||
                user.State.Contains(searchString) ||
                user.City.Contains(searchString) ||
                user.PinCode.Contains(searchString) ||
                user.Email.Contains(searchString) ||
                user.Username.Contains(searchString)
                ).ToList();
            }
            switch (sortingstring)
            {
                case "Email":
                    users = users.OrderBy(u => u.Email).ToList();
                    break;
                case "FirstNmae":
                    users = users.OrderBy(u => u.FirstName).ToList();
                    break;
                case "LastName":
                    users = users.OrderBy(u => u.LastName).ToList();
                    break;
                case "MiddleName":
                    users = users.OrderBy(u => u.MiddleName).ToList();
                    break;
                case "Address":
                    users = users.OrderBy(u => u.Address).ToList();
                    break;
                case "PhoneNumber":
                    users = users.OrderBy(u => u.PhoneNumber).ToList();
                    break;
                case "City":
                    users = users.OrderBy(u => u.City).ToList();
                    break;
                case "State":
                    users = users.OrderBy(u => u.State).ToList();
                    break;
                case "PinCode":
                    users = users.OrderBy(u => u.PinCode).ToList();
                    break;
                default:
                    users = users.OrderBy(u => u.Username).ToList();
                    break;
            }
            ViewBag.users = users;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update_user(Panacea_User user)
        {
            var p_u = await _context.P_Users.FindAsync(user.Id);
            p_u.PhoneNumber = user.PhoneNumber;
            p_u.PinCode = user.PinCode;
            p_u.State = user.State;
            p_u.City = user.City;
            p_u.IsActive = user.IsActive;
            p_u.Address = user.Address;
            _logger.LogInformation($"{p_u.LastName},{p_u.PhoneNumber}");
            var u = await _context.Users.FindAsync(user.Id);
            u.PhoneNumber = user.PhoneNumber;
            _context.P_Users.Update(p_u);
            _context.Users.Update(u);
            _logger.LogInformation($"{u.UserName},{u.PhoneNumber}");
            await _context.SaveChangesAsync();
            _logger.LogInformation($"{p_u.LastName},{p_u.PhoneNumber}");
            return RedirectToAction("User_List");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Panacea_User puser)
        {
            var userid = puser.Id;
            _logger.LogError(userid);
            var user = await _context.Users.FindAsync(userid);
            _logger.LogError(user.Id);
            var p_user = await _context.P_Users.FindAsync(userid);
            _logger.LogError(p_user.Id);
            try
            {
                _context.Users.Remove(user);
                _context.P_Users.Remove(p_user);
                await _context.SaveChangesAsync();
                return RedirectToAction("User_List");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error roshan : {ex}");
                return RedirectToAction("Error", "Home");
            }
        }
    }
}

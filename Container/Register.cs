using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Panacea_User_Management.Models;
using Panacea_User_Management.Panacea_DbContext;
using Panacea_User_Management.Services;

namespace Panacea_User_Management.Container
{
    public class _Register : IRegister
    {
        private readonly Panacea_User_Management_DbContext _context;
        private readonly UserManager<User_Model> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public _Register(Panacea_User_Management_DbContext context,
            UserManager<User_Model> userManager, RoleManager<IdentityRole> roleManager
            )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<ActionResult> Register(Panacea_User user)
        {
            //if(await _context)
            //await _roleManager.CreateAsync(new IdentityRole("Admin"));
            user.Id = user.Username;
            User_Model user_ = new User_Model();
            user_.Email = user.Email;
            user_.Id = Convert.ToString(user.Id);
            user_.PhoneNumber = user.PhoneNumber;
            user_.UserName = user.Username;
            user_.NormalizedEmail = user_.Email.ToUpper();
            user_.NormalizedUserName = user_.UserName.ToUpper();
            user_.SecurityStamp = Guid.NewGuid().ToString();
            await _userManager.CreateAsync(user_, user.Password);
            //await _context.Users.AddAsync(user_);
            await _context.P_Users.AddAsync(user);
            await _userManager.AddToRoleAsync(user_,"User");
            await _context.SaveChangesAsync();
            return new OkResult();
        }
    }
}

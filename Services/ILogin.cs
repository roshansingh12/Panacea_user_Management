using Microsoft.AspNetCore.Mvc;
using Panacea_User_Management.Models;

namespace Panacea_User_Management.Services
{
    public interface ILogin
    {
        public Task<ActionResult> _Login(UserDTO user,HttpContext _httpContext);
    }
}

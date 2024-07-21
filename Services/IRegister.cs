using Microsoft.AspNetCore.Mvc;
using Panacea_User_Management.Models;

namespace Panacea_User_Management.Services
{
    public interface IRegister
    {
        public Task<ActionResult> Register(Panacea_User user);
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Panacea_User_Management.Models;
namespace Panacea_User_Management.Panacea_DbContext
{
    public class Panacea_User_Management_DbContext : IdentityDbContext<User_Model>
    {
        public Panacea_User_Management_DbContext(DbContextOptions<Panacea_User_Management_DbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Panacea_User> P_Users { get; set; }
    }
}

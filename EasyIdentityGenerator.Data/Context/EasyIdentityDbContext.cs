using EasyIdentityGenerator.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EasyIdentityGenerator.Data.Context
{
    public class EasyIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public EasyIdentityDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
using LCM.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LCM.Persistance
{
    public class LCMContext : IdentityDbContext
    {
        public LCMContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<LCMUser> LCMUsers { get; set; }
        public DbSet<LCMHistory> LCMHistory { get; set; }
    }
}
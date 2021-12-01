using Microsoft.EntityFrameworkCore;
using ProjectApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProjectApp.Data
{
    public class ApiDbContext : IdentityDbContext
    {
        public virtual DbSet<ItemData> Items {get;set;}
        public virtual DbSet<RefreshToken> RefreshTokens {get;set;}
        public ApiDbContext(DbContextOptions<ApiDbContext>options) : base(options)
        {
            
        }
    }
}
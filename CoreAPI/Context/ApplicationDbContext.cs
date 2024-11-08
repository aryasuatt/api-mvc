using Microsoft.EntityFrameworkCore;
using CoreAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CoreAPI.Context
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet'lerinizi buraya ekleyebilirsiniz
        public DbSet<CoreAPI.Models.ToBuy> ToBuy { get; set; } = default!;
    }

    public class ApplicationUser : IdentityUser
    {
        // Ek kullanıcı özelliklerini buraya ekleyebilirsiniz
    }
}

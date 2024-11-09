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
        public DbSet<CoreAPI.Models.Image> Images { get; set; }
        public DbSet<Product> Products { get; set; } // Ürün tablosu
        public DbSet<Cart> Carts { get; set; } // Sepet tablosu
        public DbSet<CartItem> CartItems { get; set; } // Sepet öğeleri

        
    }


    public class ApplicationUser : IdentityUser
    {
        // Ek kullanıcı özelliklerini buraya ekleyebilirsiniz
    }
}

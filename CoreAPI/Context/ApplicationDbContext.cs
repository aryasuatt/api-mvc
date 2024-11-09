using Microsoft.EntityFrameworkCore;
using CoreAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

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

        //protected override void onmodelcreating(modelbuilder modelbuilder)
        //{
        //    base.onmodelcreating(modelbuilder);

        //    // product ve ımage arasında ilişki kurmak
        //    modelbuilder.entity<product>() // burada statik olmayan bir çağrı
        //        .hasone(p => p.ımage)
        //        .withmany() // bir resim birden fazla ürüne ait olabilir, ancak burada tek yönlü ilişki
        //        .hasforeignkey(p => p.ımageıd)
        //        .ondelete(deletebehavior.restrict); // silme işlemi sırasında dikkatli olun
        //}




    }


}

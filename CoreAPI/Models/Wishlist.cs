using CoreAPI.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using CoreAPI.Models;
namespace CoreAPI.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public float Price { get; set; }
        public bool IsBought { get; set; }
    }


    public static class WishlistEndpoints
    {
        public static void MapWishlistEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Wishlist").WithTags(nameof(Wishlist));

            // Tüm Wishlist öğelerini getir
            group.MapGet("/", async (ApplicationDbContext db) =>
            {
                return await db.Wishlist.ToListAsync();
            })
            .WithName("GetAllWishlistItems")
            .WithOpenApi();

            // Belirli bir Wishlist öğesini getir
            group.MapGet("/{id}", async Task<Results<Ok<Wishlist>, NotFound>> (int id, ApplicationDbContext db) =>
            {
                return await db.Wishlist.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.Id == id)
                    is Wishlist model
                        ? TypedResults.Ok(model)
                        : TypedResults.NotFound();
            })
            .WithName("GetWishlistItemById")
            .WithOpenApi();

            // Wishlist öğesini güncelle
            group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Wishlist Wishlist, ApplicationDbContext db) =>
            {
                var affected = await db.Wishlist
                    .Where(model => model.Id == id)
                    .ExecuteUpdateAsync(setters => setters
                      .SetProperty(m => m.Id, Wishlist.Id)
                      .SetProperty(m => m.Name, Wishlist.Name)
                      .SetProperty(m => m.Count, Wishlist.Count)
                      .SetProperty(m => m.Price, Wishlist.Price)
                      .SetProperty(m => m.IsBought, Wishlist.IsBought)
                    );
                return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
            })
            .WithName("UpdateWishlistItem")
            .WithOpenApi();

            // Yeni Wishlist öğesi oluştur
            group.MapPost("/", async (Wishlist Wishlist, ApplicationDbContext db) =>
            {
                db.Wishlist.Add(Wishlist);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/api/Wishlist/{Wishlist.Id}", Wishlist);
            })
            .WithName("CreateWishlistItem")
            .WithOpenApi();

            // Wishlist öğesini sil
            group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
            {
                var affected = await db.Wishlist
                    .Where(model => model.Id == id)
                    .ExecuteDeleteAsync();
                return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
            })
            .WithName("DeleteWishlistItem")
            .WithOpenApi();
        }
    }
}


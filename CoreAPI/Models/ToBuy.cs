using CoreAPI.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace CoreAPI.Models
{
    public class ToBuy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public float Price { get; set; }

        public int Month { get; set; }
        public bool IsBought { get; set; }
    }


public static class ToBuyEndpoints
{
	public static void MapToBuyEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ToBuy").WithTags(nameof(ToBuy));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.ToBuy.ToListAsync();
        })
        .WithName("GetAllToBuys")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<ToBuy>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.ToBuy.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is ToBuy model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetToBuyById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, ToBuy toBuy, ApplicationDbContext db) =>
        {
            var affected = await db.ToBuy
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, toBuy.Id)
                  .SetProperty(m => m.Name, toBuy.Name)
                  .SetProperty(m => m.Count, toBuy.Count)
                  .SetProperty(m => m.Price, toBuy.Price)
                  .SetProperty(m => m.Month, toBuy.Month)
                  .SetProperty(m => m.IsBought, toBuy.IsBought)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateToBuy")
        .WithOpenApi();

        group.MapPost("/", async (ToBuy toBuy, ApplicationDbContext db) =>
        {
            db.ToBuy.Add(toBuy);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/ToBuy/{toBuy.Id}",toBuy);
        })
        .WithName("CreateToBuy")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.ToBuy
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteToBuy")
        .WithOpenApi();
    }
}}

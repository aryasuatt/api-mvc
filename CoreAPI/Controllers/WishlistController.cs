using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreAPI.Context;
using CoreAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.ResponseCaching;


namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Wishlist
        [HttpGet]
        [Authorize]
        [ResponseCache(Duration = 60)] // Yanıtı 60 saniye cache'le
        public async Task<ActionResult<IEnumerable<Wishlist>>> GetWishlist()
        {
            return await _context.Wishlist.ToListAsync();
        }

        // GET: api/Wishlist/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wishlist>> GetWishlist(int id)
        {
            var Wishlist = await _context.Wishlist.FindAsync(id);

            if (Wishlist == null)
            {
                return NotFound();
            }

            return Wishlist;
        }

        // PUT: api/Wishlist/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWishlist(int id, Wishlist Wishlist)
        {
            if (id != Wishlist.Id)
            {
                return BadRequest();
            }

            _context.Entry(Wishlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WishlistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Wishlist
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Wishlist>> PostWishlist(Wishlist Wishlist)
        {
            _context.Wishlist.Add(Wishlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWishlist", new { id = Wishlist.Id }, Wishlist);
        }

        // DELETE: api/Wishlist/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlist(int id)
        {
            var Wishlist = await _context.Wishlist.FindAsync(id);
            if (Wishlist == null)
            {
                return NotFound();
            }

            _context.Wishlist.Remove(Wishlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WishlistExists(int id)
        {
            return _context.Wishlist.Any(e => e.Id == id);
        }
    }
}

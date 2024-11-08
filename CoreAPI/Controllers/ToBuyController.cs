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
    public class ToBuyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ToBuyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ToBuy
        [HttpGet]
        [Authorize]
        [ResponseCache(Duration = 60)] // Yanıtı 60 saniye cache'le
        public async Task<ActionResult<IEnumerable<ToBuy>>> GetToBuy()
        {
            return await _context.ToBuy.ToListAsync();
        }

        // GET: api/ToBuy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToBuy>> GetToBuy(int id)
        {
            var toBuy = await _context.ToBuy.FindAsync(id);

            if (toBuy == null)
            {
                return NotFound();
            }

            return toBuy;
        }

        // PUT: api/ToBuy/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToBuy(int id, ToBuy toBuy)
        {
            if (id != toBuy.Id)
            {
                return BadRequest();
            }

            _context.Entry(toBuy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToBuyExists(id))
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

        // POST: api/ToBuy
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToBuy>> PostToBuy(ToBuy toBuy)
        {
            _context.ToBuy.Add(toBuy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToBuy", new { id = toBuy.Id }, toBuy);
        }

        // DELETE: api/ToBuy/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToBuy(int id)
        {
            var toBuy = await _context.ToBuy.FindAsync(id);
            if (toBuy == null)
            {
                return NotFound();
            }

            _context.ToBuy.Remove(toBuy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToBuyExists(int id)
        {
            return _context.ToBuy.Any(e => e.Id == id);
        }
    }
}

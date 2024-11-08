using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreMVC.Data;
using CoreMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreMVC.Controllers
{
    
    public class ToBuyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToBuyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ToBuy
        public async Task<IActionResult> Index(ToBuy? toBuy)
        {
            if (toBuy.Name != null)
            {
                return View(await _context.ToBuy.Where(x=> x.Name.Contains(toBuy.Name)).ToListAsync());
            }
            else if (toBuy.IsBought != null)
            {
                return View(await _context.ToBuy.Where(x => x.IsBought == toBuy.IsBought).ToListAsync());
            }
            return View(await _context.ToBuy.ToListAsync());
        }

        // GET: ToBuy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toBuy = await _context.ToBuy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toBuy == null)
            {
                return NotFound();
            }

            return View(toBuy);
        }

        // GET: ToBuy/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToBuy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Count,Price,Count,IsBought")] ToBuy toBuy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toBuy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toBuy);
        }

        // GET: ToBuy/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toBuy = await _context.ToBuy.FindAsync(id);
            if (toBuy == null)
            {
                return NotFound();
            }
            return View(toBuy);
        }

        // POST: ToBuy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Count,Price,Count,IsBought")] ToBuy toBuy)
        {
            if (id != toBuy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toBuy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToBuyExists(toBuy.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(toBuy);
        }

        // GET: ToBuy/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toBuy = await _context.ToBuy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toBuy == null)
            {
                return NotFound();
            }

            return View(toBuy);
        }

        // POST: ToBuy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toBuy = await _context.ToBuy.FindAsync(id);
            if (toBuy != null)
            {
                _context.ToBuy.Remove(toBuy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToBuyExists(int id)
        {
            return _context.ToBuy.Any(e => e.Id == id);
        }
    }
}

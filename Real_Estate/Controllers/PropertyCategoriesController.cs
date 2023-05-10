using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Real_Estate.Data;
using Real_Estate.Models;

namespace Real_Estate.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PropertyCategoriesController : Controller
    {
        private readonly RealEDbContext _context;

        public PropertyCategoriesController(RealEDbContext context)
        {
            _context = context;
        }

        // GET: PropertyCategories
        public async Task<IActionResult> Index()
        {
            return _context.PropertyCategories != null ?
                        View(await _context.PropertyCategories.ToListAsync()) :
                        Problem("Entity set 'RealEDbContext.PropertyCategories'  is null.");
        }
        [Authorize(Roles = "Admin")]
        // GET: PropertyCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PropertyCategories == null)
            {
                return NotFound();
            }

            var propertyCategory = await _context.PropertyCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyCategory == null)
            {
                return NotFound();
            }

            return View(propertyCategory);
        }
        [Authorize(Roles = "Admin")]
        // GET: PropertyCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PropertyCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] PropertyCategory propertyCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(propertyCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(propertyCategory);
        }
        [Authorize(Roles = "Admin")]
        // GET: PropertyCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PropertyCategories == null)
            {
                return NotFound();
            }

            var propertyCategory = await _context.PropertyCategories.FindAsync(id);
            if (propertyCategory == null)
            {
                return NotFound();
            }
            return View(propertyCategory);
        }

        // POST: PropertyCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] PropertyCategory propertyCategory)
        {
            if (id != propertyCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(propertyCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyCategoryExists(propertyCategory.Id))
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
            return View(propertyCategory);
        }
        [Authorize(Roles = "Admin")]
        // GET: PropertyCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PropertyCategories == null)
            {
                return NotFound();
            }

            var propertyCategory = await _context.PropertyCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyCategory == null)
            {
                return NotFound();
            }

            return View(propertyCategory);
        }

        // POST: PropertyCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PropertyCategories == null)
            {
                return Problem("Entity set 'RealEDbContext.PropertyCategories'  is null.");
            }
            var propertyCategory = await _context.PropertyCategories.FindAsync(id);
            if (propertyCategory != null)
            {
                _context.PropertyCategories.Remove(propertyCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyCategoryExists(int id)
        {
            return (_context.PropertyCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

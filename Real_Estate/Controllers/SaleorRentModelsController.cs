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
    public class SaleorRentModelsController : Controller
    {
        private readonly RealEDbContext _context;

        public SaleorRentModelsController(RealEDbContext context)
        {
            _context = context;
        }

        // GET: SaleorRentModels
        public async Task<IActionResult> Index()
        {
            return _context.SaleorRentModel != null ?
                        View(await _context.SaleorRentModel.ToListAsync()) :
                        Problem("Entity set 'RealEDbContext.SaleorRentModel'  is null.");
        }

        // GET: SaleorRentModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SaleorRentModel == null)
            {
                return NotFound();
            }

            var saleorRentModel = await _context.SaleorRentModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saleorRentModel == null)
            {
                return NotFound();
            }

            return View(saleorRentModel);
        }

        // GET: SaleorRentModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SaleorRentModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SaleorRentModel saleorRentModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(saleorRentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(saleorRentModel);
        }

        // GET: SaleorRentModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SaleorRentModel == null)
            {
                return NotFound();
            }

            var saleorRentModel = await _context.SaleorRentModel.FindAsync(id);
            if (saleorRentModel == null)
            {
                return NotFound();
            }
            return View(saleorRentModel);
        }

        // POST: SaleorRentModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SaleorRentModel saleorRentModel)
        {
            if (id != saleorRentModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saleorRentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleorRentModelExists(saleorRentModel.Id))
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
            return View(saleorRentModel);
        }

        // GET: SaleorRentModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SaleorRentModel == null)
            {
                return NotFound();
            }

            var saleorRentModel = await _context.SaleorRentModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saleorRentModel == null)
            {
                return NotFound();
            }

            return View(saleorRentModel);
        }

        // POST: SaleorRentModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SaleorRentModel == null)
            {
                return Problem("Entity set 'RealEDbContext.SaleorRentModel'  is null.");
            }
            var saleorRentModel = await _context.SaleorRentModel.FindAsync(id);
            if (saleorRentModel != null)
            {
                _context.SaleorRentModel.Remove(saleorRentModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleorRentModelExists(int id)
        {
            return (_context.SaleorRentModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

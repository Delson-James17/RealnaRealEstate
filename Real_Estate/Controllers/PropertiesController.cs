using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.ViewModels;

namespace Real_Estate.Controllers
{

    public class PropertiesController : Controller
    {
        private readonly RealEDbContext _context;

        public PropertiesController(RealEDbContext context)
        {
            _context = context;
        }

        public IActionResult CreatePropertyComplete()
        {
            return View();
        }

        public IActionResult EditPropertyComplete()
        {
            return View();
        }


        // GET: Properties
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string SearchString)
        {
            var properties = await this._context.PropertyCategories.Select(p => new PropertyCategory
            {
                Id = p.Id,
                Name = p.Name

            }).ToListAsync();
            //var realEstateDbContext = _context.Properties.Include(p => p.Propertytypes).Include(p => p.owner);
            //return View(await realEstateDbContext.ToListAsync());
            ViewData["CurrentFilter"] = SearchString;
            var property = from b in _context.EstateProperties
                           select b;
            if (!String.IsNullOrEmpty(SearchString))
            {
                property = property.Where(b => b.Name.Contains(SearchString) || b.Address.Contains(SearchString) || b.PropertyCategory.Name.Contains(SearchString));
            }
            ViewData["PropertyCategoryId"] = new SelectList(properties, "Id", "Name");
            ViewData["SaleOrRentModelId"] = new SelectList(properties, "Id", "Name");
            return View(await property.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> ZoomLink(string id)
        {
            ApplicationUser? user = await _context.ApplicationUsers.FindAsync(id);
            ViewBag.User = user;
            return View();
        }
        public async Task<IActionResult> Properties(string SearchString, int? SaleOrRentModelId, int? PropertyCategoryId)
        {
            // Retrieve all property categories and sale or rent models
            var propertyCategories = await this._context.PropertyCategories.ToListAsync();
            var saleOrRentModels = await this._context.SaleorRentModel.ToListAsync();

            ViewData["CurrentFilter"] = SearchString;

            // Retrieve all estate properties
            var estateProperties = from p in _context.EstateProperties
                                   select p;

            // Apply search filters if specified
            if (!String.IsNullOrEmpty(SearchString))
            {
                estateProperties = estateProperties.Where(p =>
                    p.Name.Contains(SearchString) ||
                    p.Address.Contains(SearchString) ||
                    p.PropertyCategory.Name.Contains(SearchString) ||
                    p.SaleOrRentModel.Name.Contains(SearchString)); 
            }

            if (SaleOrRentModelId.HasValue)
            {
                estateProperties = estateProperties.Where(p => p.SaleOrRentModelId == SaleOrRentModelId.Value);
            }

            if (PropertyCategoryId.HasValue)
            {
                estateProperties = estateProperties.Where(p => p.PropertyCategoryId == PropertyCategoryId.Value);
            }

            // Create dropdown list for sale or rent model
            ViewData["SaleOrRentModelId"] = new SelectList(saleOrRentModels, "Id", "Name", SaleOrRentModelId);

            // Create dropdown list for property category
            ViewData["PropertyCategoryId"] = new SelectList(propertyCategories, "Id", "Name", PropertyCategoryId);

            return View(await estateProperties.AsNoTracking().ToListAsync());
        }

        /* public async Task<IActionResult> Properties(string SearchString)
         {
             var properties = await this._context.PropertyCategories.Select(p => new PropertyCategory
             {
                 Id = p.Id,
                 Name = p.Name

             }).ToListAsync();
             var salerent = await this._context.SaleorRentModel.Select(p => new SaleorRentModel
             {
                 Id = p.Id,
                 Name = p.Name

             }).ToListAsync();
             // var realEstateDbContext = _context.Properties.Include(p => p.Propertytypes).Include(p => p.owner);
             //return View(await realEstateDbContext.ToListAsync());

             ViewData["CurrentFilter"] = SearchString;
             var property = from b in _context.EstateProperties
                            select b;
             if (!String.IsNullOrEmpty(SearchString))
             {
                 property = property.Where(b => b.Name.Contains(SearchString)|| b.Address.Contains(SearchString)|| b.PropertyCategory.Name.Contains(SearchString)|| b.SaleOrRentModel.Name.Contains(SearchString));
             }
             ViewData["SaleOrRentModelId"] = new SelectList(salerent, "Id", "Name");
             ViewData["PropertyCategoryId"] = new SelectList(properties, "Id", "Name");
             return View(await property.AsNoTracking().ToListAsync());
         }*/
        [Authorize(Roles = "Admin, Client, Owner")]

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EstateProperties == null)
            {
                return NotFound();
            }

            var @property = await _context.EstateProperties
                .Include(a => a.ApplicationUser)
                .Include(a => a.PropertyCategory)
                .Include(a => a.SaleOrRentModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["SaleOrRentModelId"] = new SelectList(_context.SaleorRentModel, "Id", "Name");
            ViewData["PropertyCategoryId"] = new SelectList(_context.PropertyCategories, "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePropertyViewModel property)
        {
            if (ModelState.IsValid)
            {
                // Find id of currently logged in user
                string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Find user in db using its id
                ApplicationUser? user = await _context.ApplicationUsers.FindAsync(Id);

                if (user != null)
                {
                    // Map from view model to model
                    EstateProperty estateProperty = new EstateProperty()
                    {
                        Name = property.Name,
                        Description = property.Description,
                        Address = property.Address,
                        UrlImages = property.UrlImages,
                        Price = property.Price,
                        SaleOrRentModelId = property.SaleOrRentModelId,
                        OwnerName = property.OwnerName,
                        PropertyCategoryId = property.PropertyCategoryId,
                        ApplicationUserId = user.Id
                    };

                    // Create
                    await this._context.EstateProperties.AddAsync(estateProperty);

                    // Save
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                    
                }
            }
            ViewData["SaleOrRentModelId"] = new SelectList(_context.SaleorRentModel, "Id", "Name", property.SaleOrRentModelId);
            ViewData["PropertyCategoryId"] = new SelectList(_context.PropertyCategories, "Id", "Name", property.PropertyCategoryId);
            return View(property);
        }


        [HttpGet]
        public IActionResult CreatePropOwner()
        {
            ViewData["SaleOrRentModelId"] = new SelectList(_context.SaleorRentModel, "Id", "Name");
            ViewData["PropertyCategoryId"] = new SelectList(_context.PropertyCategories, "Id", "Name");
            return View();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreatePropOwner(CreatePropertyViewModel property)
        {
            if (ModelState.IsValid)
            {
                // Find id of currently logged in user
                string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Find user in db using its id
                ApplicationUser? user = await _context.ApplicationUsers.FindAsync(Id);

                if (user != null)
                {
                    // Map from view model to model
                    EstateProperty estateProperty = new EstateProperty()
                    {
                        Name = property.Name,
                        Description = property.Description,
                        Address = property.Address,
                        UrlImages = property.UrlImages,
                        Price = property.Price,
                        SaleOrRentModelId = property.SaleOrRentModelId,
                        OwnerName = property.OwnerName,
                        PropertyCategoryId = property.PropertyCategoryId,
                        ApplicationUserId = user.Id
                    };

                    // Create
                    await this._context.EstateProperties.AddAsync(estateProperty);

                    // Save
                    await _context.SaveChangesAsync();

                    //return RedirectToAction(nameof(Properties));
                    return RedirectToAction("CreatePropertyComplete", "Properties");
                }
            }
            ViewData["SaleOrRentModelId"] = new SelectList(_context.SaleorRentModel, "Id", "Name", property.SaleOrRentModelId);
            ViewData["PropertyCategoryId"] = new SelectList(_context.PropertyCategories, "Id", "Name", property.PropertyCategoryId);
            return View(property);
        }
        [Authorize(Roles = "Owner")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EstateProperties == null)
            {
                return NotFound();
            }

            var @property = await _context.EstateProperties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@property == null)
            {
                return NotFound();
            }

            // Map from model to view model
            var propertyViewModel = new EditPropertyViewModel()
            {
                Name = @property.Name,
                Description = @property.Description,
                Address = @property.Address,
                UrlImages = @property.UrlImages,
                Price = @property.Price,
                SaleOrRentModelId = @property.SaleOrRentModelId,
                OwnerName = @property.OwnerName,
                PropertyCategoryId = @property.PropertyCategoryId
            };
            ViewData["SaleOrRentModelId"] = new SelectList(_context.SaleorRentModel, "Id", "Name", property.SaleOrRentModelId);
            ViewData["PropertyCategoryId"] = new SelectList(_context.PropertyCategories, "Id", "Name", property.PropertyCategoryId);

            return View(propertyViewModel);
        }

        // POST: Properties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Edit(int id, EditPropertyViewModel property)
        {
            if (id != property.Id || _context.EstateProperties == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Find the property in the database
                var estateProperty = await _context.EstateProperties.FindAsync(id);

                if (estateProperty == null)
                {
                    return NotFound();
                }

                // Update the property with the values from the view model
                estateProperty.Name = property.Name;
                estateProperty.Description = property.Description;
                estateProperty.Address = property.Address;
                estateProperty.UrlImages = property.UrlImages;
                estateProperty.Price = property.Price;
                estateProperty.SaleOrRentModelId = property.SaleOrRentModelId;
                estateProperty.OwnerName = property.OwnerName;
                estateProperty.PropertyCategoryId = property.PropertyCategoryId;
                // Save the changes to the database
                _context.EstateProperties.Update(estateProperty);
                await _context.SaveChangesAsync();

                //return RedirectToAction(nameof(Index));
                //return RedirectToAction("UpdatedSuccessfully", "Properties");
                return RedirectToAction("EditPropertyComplete", "Properties");
            }
            ViewData["SaleOrRentModelId"] = new SelectList(_context.SaleorRentModel, "Id", "Name", property.SaleOrRentModelId);
            ViewData["PropertyCategoryId"] = new SelectList(_context.PropertyCategories, "Id", "Name", property.PropertyCategoryId);
            return View(property);
        }
        [Authorize(Roles = "Admin")]
        // GET: Properties/Edit/5
        public async Task<IActionResult> EditAdmin(int? id)
        {
            if (id == null || _context.EstateProperties == null)
            {
                return NotFound();
            }

            var @property = await _context.EstateProperties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@property == null)
            {
                return NotFound();
            }

            // Map from model to view model
            var propertyViewModel = new EditPropertyViewModel()
            {
                Name = @property.Name,
                Description = @property.Description,
                Address = @property.Address,
                UrlImages = @property.UrlImages,
                Price = @property.Price,
                SaleOrRentModelId = @property.SaleOrRentModelId,
                OwnerName = @property.OwnerName,
                PropertyCategoryId = @property.PropertyCategoryId
            };
            ViewData["SaleOrRentModelId"] = new SelectList(_context.SaleorRentModel, "Id", "Name", property.SaleOrRentModelId);
            ViewData["PropertyCategoryId"] = new SelectList(_context.PropertyCategories, "Id", "Name", property.PropertyCategoryId);

            return View(propertyViewModel);
        }

        // POST: Properties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAdmin(int id, EditPropertyViewModel property)
        {
            if (id != property.Id || _context.EstateProperties == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Find the property in the database
                var estateProperty = await _context.EstateProperties.FindAsync(id);

                if (estateProperty == null)
                {
                    return NotFound();
                }

                // Update the property with the values from the view model
                estateProperty.Name = property.Name;
                estateProperty.Description = property.Description;
                estateProperty.Address = property.Address;
                estateProperty.UrlImages = property.UrlImages;
                estateProperty.Price = property.Price;
                estateProperty.SaleOrRentModelId = property.SaleOrRentModelId;
                estateProperty.OwnerName = property.OwnerName;
                estateProperty.PropertyCategoryId = property.PropertyCategoryId;
                // Save the changes to the database
                _context.EstateProperties.Update(estateProperty);
                await _context.SaveChangesAsync();

                //return RedirectToAction(nameof(Index));
                //return RedirectToAction("UpdatedSuccessfully", "Properties");
                return RedirectToAction("Index", "Properties");
            }
            ViewData["SaleOrRentModelId"] = new SelectList(_context.SaleorRentModel, "Id", "Name", property.SaleOrRentModelId);
            ViewData["PropertyCategoryId"] = new SelectList(_context.PropertyCategories, "Id", "Name", property.PropertyCategoryId);
            return View(property);
        }

        [Authorize(Roles = "Admin, Owner")]
        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EstateProperties == null)
            {
                return NotFound();
            }

            var @property = await _context.EstateProperties
                .Include(a => a.PropertyCategory)
                .Include(a => a.SaleOrRentModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EstateProperties == null)
            {
                return Problem("Entity set 'RealEstateDbContext.Properties'  is null.");
            }
            var @property = await _context.EstateProperties.FindAsync(id);
            if (@property != null)
            {
                _context.EstateProperties.Remove(@property);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult ListByCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ListByCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var properties = await _context.EstateProperties.Include(p => p.PropertyCategory)
                .Where(p => p.PropertyCategoryId == id)
                .ToListAsync();

            if (properties == null || properties.Count == 0)
            {
                return NotFound();
            }

            var categories = await _context.PropertyCategories.ToListAsync();

            var viewModel = new PropertyListViewModel
            {
                Properties = properties,
                Categories = categories,
                Id = id.Value
            };

            return View(properties);
        }

        private bool PropertyExists(int id)
        {
            return (_context.EstateProperties?.Any(e => e.Id == id)).GetValueOrDefault();
        }


    }
}

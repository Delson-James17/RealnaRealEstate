using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.Repository.EstateProperties;
using Real_Estate.Repository.PropertyCategoryRepositories;
using Real_Estate.Repository.SalesOrRents;
using Real_Estate.ViewModels;

namespace Real_Estate.Controllers
{

    public class PropertiesController : Controller
    {
        private readonly RealEDbContext _context;
        private readonly IEstatePropertyRepository _estatePropertyRepository;
        private readonly IPropertyCategoryRepository _propertyCategoryRepository;
        private readonly ISaleOrRentRepository _saleOrRentRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PropertiesController(RealEDbContext context, 
                                    IEstatePropertyRepository estatePropertyRepository, 
                                    IPropertyCategoryRepository propertyCategoryRepository,
                                    ISaleOrRentRepository saleOrRentRepository,
                                    UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this._estatePropertyRepository = estatePropertyRepository;
            this._propertyCategoryRepository = propertyCategoryRepository;
            this._saleOrRentRepository = saleOrRentRepository;
            this._userManager = userManager;
        }

        public IActionResult CreatePropertyComplete()
        {
            return View();
        }

        public IActionResult EditPropertyComplete()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string? searchString)
        {
            var propertyCategories = await this._propertyCategoryRepository.GetAllPropertyCategoriesNames();
            var estateProperties = await this._estatePropertyRepository.GetEstatePropertiesBySearchString(searchString, null, null);
            
            ViewData["CurrentFilter"] = searchString;
            ViewData["PropertyCategoryId"] = new SelectList(propertyCategories, "Id", "Name");
            ViewData["SaleOrRentModelId"] = new SelectList(propertyCategories, "Id", "Name");

            return View(estateProperties);
        }

        public async Task<IActionResult> ZoomLink(string id)
        {
            ApplicationUser? user = await _context.ApplicationUsers.FindAsync(id);
            ViewBag.User = user;
            return View();
        }
        public async Task<IActionResult> Properties(string? searchString, int? saleOrRentModelId, int? propertyCategoryId)
        {
            var propertyCategories = await this._propertyCategoryRepository.GetAllPropertyCategories();
            var saleOrRentModels = await this._saleOrRentRepository.GetAllSaleOrRent();
            var estateProperties = await this._estatePropertyRepository.GetEstatePropertiesBySearchString(searchString, saleOrRentModelId, propertyCategoryId);

            ViewData["CurrentFilter"] = searchString;
            ViewData["SaleOrRentModelId"] = new SelectList(saleOrRentModels, "Id", "Name", saleOrRentModelId);
            ViewData["PropertyCategoryId"] = new SelectList(propertyCategories, "Id", "Name", propertyCategoryId);

            return View(estateProperties);
        }

        [Authorize(Roles = "Admin, Client, Owner")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estateProperty = await this._estatePropertyRepository.GetEstatePropertyWithRelatedEntitiesById((int)id);

            return View(estateProperty);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var propertyCategories = await this._propertyCategoryRepository.GetAllPropertyCategories();
            var saleOrRentModels = await this._saleOrRentRepository.GetAllSaleOrRent();

            ViewData["PropertyCategoryId"] = new SelectList(propertyCategories, "Id", "Name");
            ViewData["SaleOrRentModelId"] = new SelectList(saleOrRentModels, "Id", "Name");

            return View();
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePropertyViewModel newProperty)
        {
            string currentlyLoggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(currentlyLoggedInUserId == null)
            {
                return Unauthorized();
            }

            ApplicationUser? user = await _context.ApplicationUsers.FindAsync(currentlyLoggedInUserId);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                var saleOrRentModels = await this._saleOrRentRepository.GetAllSaleOrRent();
                var propertyCategories = await this._propertyCategoryRepository.GetAllPropertyCategories();

                ViewData["SaleOrRentModelId"] = new SelectList(saleOrRentModels, "Id", "Name", newProperty.SaleOrRentModelId);
                ViewData["PropertyCategoryId"] = new SelectList(propertyCategories, "Id", "Name", newProperty.PropertyCategoryId);
                return View(newProperty);
            }

            await this._estatePropertyRepository.AddEstateProperty(user.Id, newProperty);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CreatePropOwner()
        {
            var propertyCategories = await this._propertyCategoryRepository.GetAllPropertyCategories();
            var saleOrRentModels = await this._saleOrRentRepository.GetAllSaleOrRent();

            ViewData["PropertyCategoryId"] = new SelectList(propertyCategories, "Id", "Name");
            ViewData["SaleOrRentModelId"] = new SelectList(saleOrRentModels, "Id", "Name");

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreatePropOwner(CreatePropertyViewModel newProperty)
        {
            string currentlyLoggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentlyLoggedInUserId == null)
            {
                return Unauthorized();
            }

            ApplicationUser? user = await _context.ApplicationUsers.FindAsync(currentlyLoggedInUserId);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                var saleOrRentModels = await this._saleOrRentRepository.GetAllSaleOrRent();
                var propertyCategories = await this._propertyCategoryRepository.GetAllPropertyCategories();

                ViewData["SaleOrRentModelId"] = new SelectList(saleOrRentModels, "Id", "Name", newProperty.SaleOrRentModelId);
                ViewData["PropertyCategoryId"] = new SelectList(propertyCategories, "Id", "Name", newProperty.PropertyCategoryId);
                
                return View(newProperty);
            }

            await this._estatePropertyRepository.AddEstateProperty(user.Id, newProperty);

            return RedirectToAction("CreatePropertyComplete", "Properties");
        }

        [Authorize(Roles = "Owner")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estateProperty = await this._estatePropertyRepository.GetEstatePropertyById((int)id);

            var propertyViewModel = new EditPropertyViewModel()
            {
                Name = estateProperty.Name,
                Description = estateProperty.Description,
                Address = estateProperty.Address,
                UrlImages = estateProperty.UrlImages,
                Price = estateProperty.Price,
                SaleOrRentModelId = estateProperty.SaleOrRentModelId,
                OwnerName = estateProperty.OwnerName,
                PropertyCategoryId = estateProperty.PropertyCategoryId
            };

            var saleOrRentModels = await this._saleOrRentRepository.GetAllSaleOrRent();
            var propertyCategories = await this._propertyCategoryRepository.GetAllPropertyCategories();

            ViewData["SaleOrRentModelId"] = new SelectList(saleOrRentModels, "Id", "Name", estateProperty.SaleOrRentModelId);
            ViewData["PropertyCategoryId"] = new SelectList(propertyCategories, "Id", "Name", estateProperty.PropertyCategoryId);

            return View(propertyViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Edit(int id, EditPropertyViewModel estatePropertyToBeUpdated)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != estatePropertyToBeUpdated.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var saleOrRentModels = await this._saleOrRentRepository.GetAllSaleOrRent();
                var propertyCategories = await this._propertyCategoryRepository.GetAllPropertyCategories();

                ViewData["SaleOrRentModelId"] = new SelectList(saleOrRentModels, "Id", "Name", estatePropertyToBeUpdated.SaleOrRentModelId);
                ViewData["PropertyCategoryId"] = new SelectList(propertyCategories, "Id", "Name", estatePropertyToBeUpdated.PropertyCategoryId);

                return View(estatePropertyToBeUpdated);
            }

            await this._estatePropertyRepository.UpdateEstateProperty(id, estatePropertyToBeUpdated);

            return RedirectToAction("EditPropertyComplete", "Properties");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estateProperty = await this._estatePropertyRepository.GetEstatePropertyById((int)id);

            var propertyViewModel = new EditPropertyViewModel()
            {
                Name = estateProperty.Name,
                Description = estateProperty.Description,
                Address = estateProperty.Address,
                UrlImages = estateProperty.UrlImages,
                Price = estateProperty.Price,
                SaleOrRentModelId = estateProperty.SaleOrRentModelId,
                OwnerName = estateProperty.OwnerName,
                PropertyCategoryId = estateProperty.PropertyCategoryId
            };

            var saleOrRentModels = await this._saleOrRentRepository.GetAllSaleOrRent();
            var propertyCategories = await this._propertyCategoryRepository.GetAllPropertyCategories();

            ViewData["SaleOrRentModelId"] = new SelectList(saleOrRentModels, "Id", "Name", estateProperty.SaleOrRentModelId);
            ViewData["PropertyCategoryId"] = new SelectList(propertyCategories, "Id", "Name", estateProperty.PropertyCategoryId);

            return View(propertyViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAdmin(int id, EditPropertyViewModel estatePropertyToBeUpdated)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != estatePropertyToBeUpdated.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var saleOrRentModels = await this._saleOrRentRepository.GetAllSaleOrRent();
                var propertyCategories = await this._propertyCategoryRepository.GetAllPropertyCategories();

                ViewData["SaleOrRentModelId"] = new SelectList(saleOrRentModels, "Id", "Name", estatePropertyToBeUpdated.SaleOrRentModelId);
                ViewData["PropertyCategoryId"] = new SelectList(propertyCategories, "Id", "Name", estatePropertyToBeUpdated.PropertyCategoryId);

                return View(estatePropertyToBeUpdated);
            }

            await this._estatePropertyRepository.UpdateEstateProperty(id, estatePropertyToBeUpdated);

            return RedirectToAction("Index", "Properties");
        }

        [Authorize(Roles = "Admin, Owner")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estateProperty = await this._estatePropertyRepository.GetEstatePropertyWithRelatedEntitiesById((int)id);

            return View(estateProperty);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this._estatePropertyRepository.DeleteEstateProperty(id);

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

            var estateProperties = await this._estatePropertyRepository.GetEstatePropertiesByPropertyCategory((int)id);

            if (estateProperties == null || estateProperties.Count == 0)
            {
                return NotFound("No Estate Properties available!");
            }

            var propertyCategories = await this._propertyCategoryRepository.GetAllPropertyCategories();

            var propertyListViewModel = new PropertyListViewModel
            {
                Properties = estateProperties,
                Categories = propertyCategories,
                Id = id.Value
            };

            return View(propertyListViewModel);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Real_Estate.Models;
using Real_Estate.Repository.PropertyCategoryRepositories;

namespace Real_Estate.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PropertyCategoriesController : Controller
    {
        private readonly IPropertyCategoryRepository _propertyCategoryRepository;

        public PropertyCategoriesController(IPropertyCategoryRepository propertyCategoryRepository)
        {
            this._propertyCategoryRepository = propertyCategoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var propertyCategories = await this._propertyCategoryRepository.GetAllPropertyCategories();
            return View(propertyCategories);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyCategory = await this._propertyCategoryRepository.GetPropertyCategoryById((int)id);

            return View(propertyCategory);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] PropertyCategory propertyCategory)
        {
            if (ModelState.IsValid)
            {
                return View(propertyCategory);
            }

            await this._propertyCategoryRepository.AddPropertyCategory(propertyCategory);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyCategory = await this._propertyCategoryRepository.GetPropertyCategoryById((int)id);

            return View(propertyCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] PropertyCategory propertyCategory)
        {
            if (id != propertyCategory.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(propertyCategory);
            }

            await this._propertyCategoryRepository.UpdatePropertyCategory(id, propertyCategory);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyCategory = await this._propertyCategoryRepository.GetPropertyCategoryById((int)id);

            return View(propertyCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            await this._propertyCategoryRepository.DeletePropertyCategoryById(id);

            return RedirectToAction(nameof(Index));
        }
    }
}

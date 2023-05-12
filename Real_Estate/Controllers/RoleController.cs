using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Real_Estate.ViewModels;
using System.Data;

namespace Real_Estate.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        public RoleManager<IdentityRole> _roleManager { get; }
        
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = roleViewModel.Name
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(roleViewModel);
        }
        [HttpGet]
        public IActionResult GetAllRoles()
        {
            return View(_roleManager.Roles.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> Update(string roleId)
        {
            var oldProperty = await _roleManager.FindByIdAsync(roleId);
            return View(oldProperty);
        }
        [HttpPost]
        public async Task<IActionResult> Update(RoleViewModel role)
        {
            var oldRole = await _roleManager.FindByIdAsync(role.Id.ToString());
            oldRole.Name = role.Name;
            var result = await _roleManager.UpdateAsync(oldRole);
            if (result.Succeeded)
            {
                return RedirectToAction("GetAllRoles");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }
        public async Task<IActionResult> Details(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            return View(role);
        }
        public async Task<IActionResult> Delete(Guid roleId)
        {
            var oldRole = await _roleManager.FindByIdAsync(roleId.ToString());

            var proplist = await _roleManager.DeleteAsync(oldRole);
            return RedirectToAction(controllerName: "Role", actionName: "GetAllRoles"); // reload the getall page it self
        }
    }
}

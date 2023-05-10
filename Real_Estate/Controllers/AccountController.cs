using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Real_Estate.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager { get; set; }
        private SignInManager<ApplicationUser> _signInManager { get; set; }
        private RoleManager<IdentityRole> _roleManager { get; set; }

        RealEDbContext _context;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, RealEDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }
        public IActionResult Complete()
        {
            return View();
        }

        public IActionResult UpdatePasswordSuccess()
        {
            return View();
        }

        public async Task<IActionResult> ListOfOwners()
        {
            List<ApplicationUser> user = (List<ApplicationUser>)await _userManager.GetUsersInRoleAsync("Owner");
            return View(user);
        }
        public async Task<IActionResult> ListOfClients()
        {
            List<ApplicationUser> user = (List<ApplicationUser>)await _userManager.GetUsersInRoleAsync("Client");
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            List<IdentityRole> roles = await _context.Roles.ToListAsync();
            IdentityRole? role = roles.Where(r => r.Name == "Admin").FirstOrDefault();
            if (role is null)
            {
                return View();
            }
            else
            {
                roles.Remove(role!);
            }

            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel userViewModel)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    var userModel = new ApplicationUser
                    {
                        Name = userViewModel.Name,
                        Address = userViewModel.Address,
                        Age = userViewModel.Age,
                        DOB = userViewModel.DOB,
                        PhoneNumber = userViewModel.PhoneNumber,
                        UrlImages = userViewModel.UrlImages,
                        UserName = userViewModel.Email,
                        Email = userViewModel.Email,

                    };
                    var result = await _userManager.CreateAsync(userModel, userViewModel.Password);
                    if (result.Succeeded)
                    {
                        IdentityRole? role = await _roleManager.Roles
                            .Where(e => e.Id == userViewModel.RoleViewModelID.ToString())
                            .FirstOrDefaultAsync();
                        if (role != null)
                        {
                            //var roleResult = await _userManager.AddToRolesAsync(userModel, roles.Select(s => s.Name).ToList());
                            var roleResult = await _userManager.AddToRoleAsync(userModel, role.Name);
                            if (!roleResult.Succeeded)
                            {
                                ModelState.AddModelError(String.Empty, "User Role cannot be assigned");
                            }
                        }
                        await _signInManager.SignInAsync(userModel, isPersistent: false);
                        return RedirectToAction("Complete", "Account");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View(userViewModel);
            }
            catch (Exception ex)
            {
                return View();
            }

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                // login activity -> cookie [Roles and Claims]
                var result = await _signInManager.PasswordSignInAsync(userViewModel.Email, userViewModel.Password, userViewModel.RememberMe, false);
                //login cookie and transfter to the client 
                if (result.Succeeded)
                {
                    return RedirectToAction("Properties", "Properties");
                }
                ModelState.AddModelError(string.Empty, "invalid login credentials");
            }
            return View(userViewModel);
        }

        private async Task<string> ValidateAndGetFullNameAsync(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                var passwordValid = await _userManager.CheckPasswordAsync(user, password);

                if (passwordValid)
                {
                    return $"{user.Name}";
                }
            }

            return null;
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                await _signInManager.RefreshSignInAsync(user);
                //return RedirectToAction("Login", "Account");
                return RedirectToAction("UpdatePasswordSuccess", "Account");
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Profile()
        {
            var user = await this._userManager.GetUserAsync(this._signInManager.Context.User);
            return View(user);
        }
        public async Task<IActionResult> ProfileAdmin()
        {
            var user = await this._userManager.GetUserAsync(this._signInManager.Context.User);
            return View(user);
        }



    }
}




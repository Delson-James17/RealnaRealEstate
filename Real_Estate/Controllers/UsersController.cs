using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.Repository.Users;
using Real_Estate.ViewModels;
using System.Data;
using System.Security.Claims;

namespace Real_Estate.Controllers
{

    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RealEDbContext _context;
        private readonly IUsersRepository _usersRepository;

        public UsersController(UserManager<ApplicationUser> userManager, 
                                RealEDbContext context,
                                IUsersRepository usersRepository)
        {
            _userManager = userManager;
            _context = context;
            this._usersRepository = usersRepository;
        }

        public IActionResult UpdatedSuccessfully()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await this._usersRepository.GetAllRegisteredUsers();
            return View(users);
        }

        public async Task<IActionResult> Details(string userId)
        {
            var user = await this._usersRepository.GetUserById(userId);
            return View(user);
        }

        public async Task<IActionResult> Delete(string userId)
        {
            var deletedUser = await this._usersRepository.DeleteUserById(userId);
            return RedirectToAction(controllerName: "Users", actionName: "GetAllUsers"); // reload the getall page it self
        }

        [HttpGet]
        public async Task<IActionResult> Update(string userId)
        {
            var userWithRole = await this._usersRepository.GetUserWithRoleById(userId);

            EditUserViewModel userViewModel = new EditUserViewModel()
            {
                Name = userWithRole.Name,
                Age = userWithRole.Age,
                Address = userWithRole.Address,
                DOB = (DateTime)userWithRole.DOB,
                PhoneNumber = userWithRole.PhoneNumber,
                UrlImages = userWithRole.UrlImages,
                Zoomlink = userWithRole.Zoomlink,
                Roles = userWithRole.Roles
            };

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditUserViewModel user)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var updateUserStatus = await this._usersRepository.UpdateUser(userId, user);

            if (updateUserStatus.Succeeded)
            {
                return RedirectToAction("UpdatedSuccessfully", "Users");
            }
            else
            {
                foreach (var error in updateUserStatus.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
                return View();
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> UpdateAdmin(string userId)
        {   
            var userWithRole = await this._usersRepository.GetUserWithRoleById(userId);

            EditUserViewModel userViewModel = new EditUserViewModel()
            {
                Name = userWithRole.Name,
                Age = userWithRole.Age,
                Address = userWithRole.Address,
                DOB = (DateTime)userWithRole.DOB,
                PhoneNumber = userWithRole.PhoneNumber,
                UrlImages = userWithRole.UrlImages,
                Zoomlink = userWithRole.Zoomlink,
                Roles = userWithRole.Roles
            };

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdmin(EditUserViewModel user)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var updateUserStatus = await this._usersRepository.UpdateUser(userId, user);

            if (updateUserStatus.Succeeded)
            {
                return RedirectToAction("UpdatedSuccessfully", "Users");
            }
            else
            {
                foreach (var error in updateUserStatus.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View();
            }
        }
    }
}


using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.ViewModels;

namespace Real_Estate.Repository.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly RealEDbContext _realEDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersRepository(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<ApplicationUser> DeleteUserById(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

            if(user != null)
            {
                var userlist = await _userManager.DeleteAsync(user);
                return user;
            }
            
            return null;
        }

        public async Task<List<ApplicationUser>> GetAllRegisteredUsers()
        {
            return await this._userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await this._userManager.Users.FirstOrDefaultAsync(user => user.Id == userId);
        }

        public async Task<UserWithRoleViewModel> GetUserWithRoleById(string userId)
        {
            var users = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            var roles = await _userManager.GetRolesAsync(users);
            UserWithRoleViewModel userWithRoleViewModel = new UserWithRoleViewModel()
            {
                Name = users.Name,
                Age = users.Age,
                Address = users.Address,
                DOB = (DateTime)users.DOB,
                PhoneNumber = users.PhoneNumber,
                UrlImages = users.UrlImages,
                Zoomlink = users.Zoomlink,
                Roles = roles
            };

            return userWithRoleViewModel;
        }

        public async Task<ApplicationUser> UpdateAdmin(string userId, EditUserViewModel editUserViewModel)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> UpdateUser(string userId, EditUserViewModel editUserViewModel)
        {
            IdentityResult? updateUserStatus = null;
            ApplicationUser? userProfile = await this._realEDbContext.ApplicationUsers.FindAsync(userId);

            if(userProfile != null)
            {
                userProfile.Name = editUserViewModel.Name;
                userProfile.Age = editUserViewModel.Age;
                userProfile.Address = editUserViewModel.Address;
                userProfile.DOB = editUserViewModel.DOB;
                userProfile.Zoomlink = editUserViewModel.Zoomlink;
                userProfile.PhoneNumber = editUserViewModel.PhoneNumber;

                updateUserStatus = await _userManager.UpdateAsync(userProfile);
            }

            return updateUserStatus;
        }
    }
}
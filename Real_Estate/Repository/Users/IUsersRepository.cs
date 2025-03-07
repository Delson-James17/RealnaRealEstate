using Microsoft.AspNetCore.Identity;
using Real_Estate.Models;
using Real_Estate.ViewModels;

namespace Real_Estate.Repository.Users
{
    public interface IUsersRepository
    {
        Task<List<ApplicationUser>> GetAllRegisteredUsers();
        Task<ApplicationUser> GetUserById(string userId);
        Task<UserWithRoleViewModel> GetUserWithRoleById(string userId);
        Task<ApplicationUser> DeleteUserById(string userId);
        Task<IdentityResult> UpdateUser(string userId, EditUserViewModel editUserViewModel);
        Task<ApplicationUser> UpdateAdmin(string userId, EditUserViewModel editUserViewModel);
    }
}
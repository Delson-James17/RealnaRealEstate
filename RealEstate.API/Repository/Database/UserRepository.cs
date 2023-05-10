using Microsoft.AspNetCore.Identity;
using RealEstate.API.Models;
using System.Security.Claims;

namespace RealEstate.API.Repository.Database
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserRepository(UserManager<ApplicationUser> _userManager,
                                IHttpContextAccessor contextAccessor)
        {
            this._userManager = _userManager;
            this._contextAccessor = contextAccessor;
        }

        public async Task<ApplicationUser> GetCurrentUser()
        {
            string userEmail = this._contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            ApplicationUser applicationUser = await this._userManager.FindByEmailAsync(userEmail);
            return applicationUser;
        }
    }
}

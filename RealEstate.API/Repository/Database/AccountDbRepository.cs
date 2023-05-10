using Microsoft.AspNetCore.Identity;
using RealEstate.API.DTO;
using RealEstate.API.Models;

namespace RealEstate.API.Repository.Database
{
    public class AccountDbRepository : IAccountRepository
    {
        public UserManager<ApplicationUser> _userManager { get; }
        public SignInManager<ApplicationUser> _signInManager { get; }

        public AccountDbRepository( UserManager<ApplicationUser> userManager, 
                                    SignInManager<ApplicationUser>signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<ApplicationUser> FindUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<SignInResult> SignInUserAsync(LoginUserDto loginUserDto)
        {
            var loginResult = await _signInManager.PasswordSignInAsync(loginUserDto.Email, loginUserDto.Password, loginUserDto.RememberMe, false);
            return loginResult;
        }
        
        public async Task<ApplicationUser> SignUpUserAsync(ApplicationUser user, string password)
        {
            var newUser = await _userManager.CreateAsync(user, password);
            if (newUser.Succeeded)
            {
                return user;
            }
            return null;
        }
    }
}

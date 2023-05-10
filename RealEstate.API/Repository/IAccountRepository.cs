using Microsoft.AspNetCore.Identity;
using RealEstate.API.DTO;
using RealEstate.API.Models;

namespace RealEstate.API.Repository
{
    public interface IAccountRepository
    {
        Task<ApplicationUser> SignUpUserAsync(ApplicationUser user, string password);
        Task<SignInResult> SignInUserAsync(LoginUserDto loginUserDto);
        Task<ApplicationUser> FindUserByEmailAsync(string email);


    }
}

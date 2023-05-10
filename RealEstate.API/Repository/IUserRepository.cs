using RealEstate.API.Models;

namespace RealEstate.API.Repository
{
    public interface IUserRepository
    {
       Task<ApplicationUser> GetCurrentUser();
    }
    
}

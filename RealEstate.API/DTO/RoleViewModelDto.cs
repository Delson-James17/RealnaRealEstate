using RealEstate.API.DTO;
using RealEstate.API.Models;

namespace RealEstate.API.DTO
{
    public class RoleViewModelDto
    {
        public Guid Id { get; set; } // Global UniqueID MAC + Timestamp
        public string Name { get; set; }
        public bool? IsSelected { get; set; }
      
        public List<RegisterUserDto>? User { get; set; }
    }
}


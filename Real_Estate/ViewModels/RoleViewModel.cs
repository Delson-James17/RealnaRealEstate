using Real_Estate.Models;

namespace Real_Estate.ViewModels
{
    public class RoleViewModel
    {
        public Guid Id { get; set; } // Global UniqueID MAC + Timestamp
        public string Name { get; set; }
        public bool? IsSelected { get; set; }
      
        public List<RegisterUserViewModel>? User { get; set; }
    }
}


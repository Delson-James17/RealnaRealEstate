using System.ComponentModel.DataAnnotations;

namespace Real_Estate.ViewModels
{
    public class UserWithRoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
        public string UserName { get; set; }
        public string UrlImages { get; set; }
        public string? Zoomlink { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public IList<string> Roles { get; set; }
    }
}

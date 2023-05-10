using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RealEstate.API.DTO
{
    public class RegisterUserDto
    {
        public string Name { get; set; }
    
        public string Address { get; set; }
  
        public int Age { get; set; }

        [RegularExpression("^(09|\\+639)\\d{9}$", ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }
   
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DOB { get; set; }
      
        public string UserName { get; set; }
   
        public string UrlImages { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirm password doesnt match")]
        public string ConfirmPassword { get; set; }
        public Guid RoleViewModelID { get; set; }
       
    }
}

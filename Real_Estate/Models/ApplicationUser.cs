using Microsoft.AspNetCore.Identity;
using Real_Estate.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Full Name")]
        public string Name { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        public int Age { get; set; }

        [DisplayName("Birth of Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        [DisplayName("Add Profile Picture Using Link")]
        public string UrlImages { get; set; }

        [DisplayName("Zoom Link")]
        public string? Zoomlink { get; set; }

        public List<OwnerSchedule> OwnerScedules { get; set; }

        public List<EstateProperty> Properties { get; set; }
    }
}

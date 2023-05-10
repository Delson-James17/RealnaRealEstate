using RealEstate.API.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.API.Models
{
    public class EstateProperty
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Full Name")]

        public string Name { get; set; }
        [DisplayName("Description")]

        public string Description { get; set; }
        [DisplayName("Address")]

        public string Address { get; set; }
        [DisplayName("Property Image")]

        public string UrlImages { get; set; }
        public int? SaleOrRentModelId { get; set; }
        public SaleorRentModel? SaleOrRentModel { get; set; }
        [DisplayName("Price")]

        public Double Price { get; set; }
        [DisplayName("Owner")]
        public string? OwnerName { get; set; }
        public List<PropertyListViewModel>? PropertyListViews { get; set; }
        [DisplayName("Select Category")]

        public int? PropertyCategoryId { get; set; }
        public PropertyCategory? PropertyCategory { get; set; }
        //relationships
        public string ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}

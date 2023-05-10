using Microsoft.Build.Framework;
using Real_Estate.Models;
using System.ComponentModel;

namespace Real_Estate.ViewModels
{
    public class EditPropertyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string UrlImages { get; set; }
        public int? SaleOrRentModelId { get; set; }
        public SaleorRentModel? SaleOrRentModel { get; set; }
        [DisplayName("Price")]

        public Double Price { get; set; }
        [DisplayName("Owner")]
        public string? OwnerName { get; set; }
        public int? PropertyCategoryId { get; set; }
        public PropertyCategory? PropertyCategory { get; set; }
    }
}

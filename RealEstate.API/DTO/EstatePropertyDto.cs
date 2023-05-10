using RealEstate.API.Models;
using System.ComponentModel;

namespace RealEstate.API.DTO
{
    public class EstatePropertyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string UrlImages { get; set; }
        public int? SaleOrRentModelId { get; set; }

        [DisplayName("Price")]
        public Double Price { get; set; }
        [DisplayName("Owner")]
        public string? OwnerName { get; set; }
        public int? PropertyCategoryId { get; set; }

    }
}

using RealEstate.API.Models;

namespace RealEstate.API.Repository
{
    public interface ISaleorRentRepository
    {
        Task<List<SaleorRentModel>> GetSaleorRentCategory();

        Task<SaleorRentModel?> GetSaleorRentCategoryById(int Id);

        Task<SaleorRentModel?> AddSaleorRentCategory(SaleorRentModel newsaleorrentcat);

        SaleorRentModel UpdateSaleorRentCategory(int propertyId, SaleorRentModel newsaleorrentcat);

        Task DeleteSaleorRentCategory(int propertyId);
    }
}

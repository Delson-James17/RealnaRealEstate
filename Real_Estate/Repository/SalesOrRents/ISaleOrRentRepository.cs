using Real_Estate.Models;

namespace Real_Estate.Repository.SalesOrRents
{
    public interface ISaleOrRentRepository
    {
        Task<List<SaleorRentModel>> GetAllSaleOrRent();
    }
}

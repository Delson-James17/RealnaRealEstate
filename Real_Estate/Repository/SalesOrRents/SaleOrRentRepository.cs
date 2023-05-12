using Microsoft.EntityFrameworkCore;
using Real_Estate.Data;
using Real_Estate.Models;

namespace Real_Estate.Repository.SalesOrRents
{
    public class SaleOrRentRepository : ISaleOrRentRepository
    {
        private readonly RealEDbContext _realEDbContext;

        public SaleOrRentRepository(RealEDbContext realEDbContext)
        {
            this._realEDbContext = realEDbContext;
        }

        public async Task<List<SaleorRentModel>> GetAllSaleOrRent()
        {
            return await this._realEDbContext.SaleorRentModel.ToListAsync();
        }
    }
}

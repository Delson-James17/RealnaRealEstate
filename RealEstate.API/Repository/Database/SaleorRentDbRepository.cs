using Microsoft.EntityFrameworkCore;
using RealEstate.API.Data;
using RealEstate.API.Models;

namespace RealEstate.API.Repository.Database
{
    public class SaleorRentDbRepository : ISaleorRentRepository
    {
        private readonly RealEDbContext _dbContext;
        public SaleorRentDbRepository(RealEDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<SaleorRentModel?> AddSaleorRentCategory(SaleorRentModel newsaleorrentcat)
        {
            await _dbContext.AddAsync(newsaleorrentcat);
            await _dbContext.SaveChangesAsync();
            return newsaleorrentcat;
        }

        public Task DeleteSaleorRentCategory(int propertyId)
        {
            var getproperty = this._dbContext.SaleorRentModel.FindAsync(propertyId);
            if (getproperty.Result != null)
            {
                this._dbContext.SaleorRentModel.Remove(getproperty.Result);
            }


            return this._dbContext.SaveChangesAsync();
        }

        public async Task<List<SaleorRentModel>> GetSaleorRentCategory()
        {
            return await _dbContext.SaleorRentModel.ToListAsync();
        }

        public Task<SaleorRentModel?> GetSaleorRentCategoryById(int Id)
        {
            var getproperty = this._dbContext.SaleorRentModel
                      .FirstOrDefaultAsync(m => m.Id == Id);

            if (getproperty == null)
            {
                return null;
            }

            return getproperty;
        }


        public SaleorRentModel UpdateSaleorRentCategory(int propertyId, SaleorRentModel newsaleorrentcat)
        {
            _dbContext.Update(newsaleorrentcat);
            _dbContext.SaveChanges();
            return newsaleorrentcat;
        }
    }
}



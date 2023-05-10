using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.API.Data;
using RealEstate.API.Models;

namespace RealEstate.API.Repository
{
    public class PropertyCategoryDbRepository : IPropertyCategoryRepository
    {
        private readonly RealEDbContext _dbContext;
        public PropertyCategoryDbRepository(RealEDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<PropertyCategory?> AddPropertyCategory(PropertyCategory newcategory)
        {
           await _dbContext.AddAsync(newcategory);
            await _dbContext.SaveChangesAsync();
            return newcategory;
        }

        public Task DeletePropertyCategory(int propertyId)
        {
            var getproperty = this._dbContext.PropertyCategories.FindAsync(propertyId);
            if (getproperty.Result != null)
            {
                this._dbContext.PropertyCategories.Remove(getproperty.Result);
            }


            return this._dbContext.SaveChangesAsync();
        }

        public async Task<List<PropertyCategory>> GetAllPropertyCategory()
        {
            return await _dbContext.PropertyCategories.ToListAsync();
        }

        public Task<PropertyCategory?> GetPropertyCategoryById(int Id)
        {
            var getproperty = this._dbContext.PropertyCategories
                     .FirstOrDefaultAsync(m => m.Id == Id);

            if (getproperty == null)
            {
                return null;
            }

            return getproperty;
        }

        public PropertyCategory UpdatePropertyCategory(int propertyId, PropertyCategory newcategory)
        {
            _dbContext.Update(newcategory);
            _dbContext.SaveChanges();
            return newcategory;
        }
    }
}

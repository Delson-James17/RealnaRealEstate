using Microsoft.EntityFrameworkCore;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.Repository.PropertyCategoryRepositories;

namespace Real_Estate.Repository.PropertyCategories
{
    public class PropertyCategoryRepository : IPropertyCategoryRepository
    {
        private readonly RealEDbContext _realEDbContext;

        public PropertyCategoryRepository(RealEDbContext realEDbContext)
        {
            this._realEDbContext = realEDbContext;
        }

        public async Task<PropertyCategory> AddPropertyCategory(PropertyCategory newPropertyCategory)
        {
            await this._realEDbContext.PropertyCategories.AddAsync(newPropertyCategory);
            await this._realEDbContext.SaveChangesAsync();
            return newPropertyCategory;
        }

        public async Task<PropertyCategory> DeletePropertyCategoryById(int propertyCategoryId)
        {
            var propertyCategory = await this._realEDbContext.PropertyCategories.FindAsync(propertyCategoryId);

            if (propertyCategory != null)
            {
                this._realEDbContext.PropertyCategories.Remove(propertyCategory);
                await this._realEDbContext.SaveChangesAsync();

                return propertyCategory;
            }

            return new PropertyCategory();
        }

        public async Task<List<PropertyCategory>> GetAllPropertyCategories()
        {
            return await this._realEDbContext.PropertyCategories
                .ToListAsync();
        }

        public async Task<List<PropertyCategory>> GetAllPropertyCategoriesNames()
        {
            return await this._realEDbContext.PropertyCategories
                .Select(propertyCategory => new PropertyCategory {
                    Id = propertyCategory.Id,
                    Name = propertyCategory.Name
                })
                .ToListAsync();
        }

        public async Task<PropertyCategory> GetPropertyCategoryById(int propertyCategoryId)
        {
            var propertyCategory = await this._realEDbContext.PropertyCategories
                .FirstOrDefaultAsync(m => m.Id == propertyCategoryId);

            return propertyCategory ?? new PropertyCategory();
        }

        public async Task<PropertyCategory> UpdatePropertyCategory(int propertyCategoryId, PropertyCategory propertyCategoryToUpdate)
        {
            this._realEDbContext.PropertyCategories.Update(propertyCategoryToUpdate);
            await this._realEDbContext.SaveChangesAsync();

            return propertyCategoryToUpdate;
        }
    }
}

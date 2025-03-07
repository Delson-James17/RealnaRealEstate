using Microsoft.EntityFrameworkCore;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.ViewModels;

namespace Real_Estate.Repository.EstateProperties
{
    public class EstatePropertyRepository : IEstatePropertyRepository
    {
        private readonly RealEDbContext _realEDbContext;

        public EstatePropertyRepository(RealEDbContext realEDbContext)
        {
            this._realEDbContext = realEDbContext;
        }

        public async Task<EstateProperty> AddEstateProperty(string userId, CreatePropertyViewModel newEstateProperty)
        {
            EstateProperty estateProperty = new EstateProperty()
            {
                Name = newEstateProperty.Name,
                Description = newEstateProperty.Description,
                Address = newEstateProperty.Address,
                UrlImages = newEstateProperty.UrlImages,
                Price = newEstateProperty.Price,
                SaleOrRentModelId = newEstateProperty.SaleOrRentModelId,
                OwnerName = newEstateProperty.OwnerName,
                PropertyCategoryId = newEstateProperty.PropertyCategoryId,
                ApplicationUserId = userId
            };

            await this._realEDbContext.EstateProperties.AddAsync(estateProperty);
            await _realEDbContext.SaveChangesAsync();

            return estateProperty;
        }

        public async Task<List<EstateProperty>> GetAllEstateProperties()
        {
            return await this._realEDbContext.EstateProperties.ToListAsync();
        }

        public async Task<List<EstateProperty>> GetEstatePropertiesBySearchString(string? searchString, int? saleOrRentModelId, int? propertyCategoryId)
        {
            var estatePropertiesQuery = this._realEDbContext.EstateProperties.AsQueryable<EstateProperty>();

            if (!String.IsNullOrEmpty(searchString))
            {
                estatePropertiesQuery = estatePropertiesQuery.Where(
                    estateProperty => estateProperty.Name.Contains(searchString) ||
                    estateProperty.Address.Contains(searchString) ||
                    estateProperty.PropertyCategory.Name.Contains(searchString)
                );
            }

            if (saleOrRentModelId != null || saleOrRentModelId.HasValue)
            {
                estatePropertiesQuery = estatePropertiesQuery
                    .Where(estateProperty => estateProperty.SaleOrRentModelId == saleOrRentModelId.Value);
            }

            if (propertyCategoryId != null || propertyCategoryId.HasValue)
            {
                estatePropertiesQuery = estatePropertiesQuery
                    .Where(estateProperty => estateProperty.PropertyCategoryId == propertyCategoryId.Value);
            }

            var estatePropertiesList  = await estatePropertiesQuery.ToListAsync();

            return estatePropertiesList;
        }

        public async Task<int> GetEstatePropertiesCount()
        {
            return await this._realEDbContext.EstateProperties.CountAsync();
        }

        public async Task<EstateProperty> GetEstatePropertyWithRelatedEntitiesById(int estatePropertyId)
        {
            var estateProperty = await this._realEDbContext.EstateProperties
                .Include(a => a.ApplicationUser)
                .Include(a => a.PropertyCategory)
                .Include(a => a.SaleOrRentModel)
                .FirstOrDefaultAsync(m => m.Id == estatePropertyId);

            return estateProperty ?? new EstateProperty();
        }

        public async Task<EstateProperty> GetEstatePropertyById(int estatePropertyId)
        {
            var estateProperty = await this._realEDbContext.EstateProperties
                .FirstOrDefaultAsync(m => m.Id == estatePropertyId);

            return estateProperty ?? new EstateProperty();
        }

        public async Task<EstateProperty> UpdateEstateProperty(int estatePropertyId, EditPropertyViewModel estatePropertyToBeUpdated)
        {
            var estateProperty = await this._realEDbContext.EstateProperties.FindAsync(estatePropertyId);

            if (estateProperty != null)
            {
                estateProperty.Name = estatePropertyToBeUpdated.Name;
                estateProperty.Description = estatePropertyToBeUpdated.Description;
                estateProperty.Address = estatePropertyToBeUpdated.Address;
                estateProperty.UrlImages = estatePropertyToBeUpdated.UrlImages;
                estateProperty.Price = estatePropertyToBeUpdated.Price;
                estateProperty.SaleOrRentModelId = estatePropertyToBeUpdated.SaleOrRentModelId;
                estateProperty.OwnerName = estatePropertyToBeUpdated.OwnerName;
                estateProperty.PropertyCategoryId = estatePropertyToBeUpdated.PropertyCategoryId;

                this._realEDbContext.EstateProperties.Update(estateProperty);
                await this._realEDbContext.SaveChangesAsync();

                return estateProperty;
            }
            
            return null;
        }

        public async Task<EstateProperty> DeleteEstateProperty(int estatePropertyId)
        {
            var estatePropertyToBeDeleted = await this._realEDbContext.EstateProperties.FindAsync(estatePropertyId);

            if (estatePropertyToBeDeleted != null)
            {
                this._realEDbContext.EstateProperties.Remove(estatePropertyToBeDeleted);
                await this._realEDbContext.SaveChangesAsync();
                return estatePropertyToBeDeleted;
            }

            return null;
        }

        public async Task<List<EstateProperty>> GetEstatePropertiesByPropertyCategory(int propertyCategoryId)
        {
            return await this._realEDbContext.EstateProperties
                .Include(estateProperty => estateProperty.PropertyCategory)
                .Where(estateProperty => estateProperty.PropertyCategoryId == propertyCategoryId)
                .ToListAsync();
        }

        public async Task<EstateProperty> GetEstatePropertyWithUserById(int estatePropertyId)
        {
            var estateProperty = await this._realEDbContext.EstateProperties
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(a => a.Id == estatePropertyId);

            return estateProperty;
        }
    }
}

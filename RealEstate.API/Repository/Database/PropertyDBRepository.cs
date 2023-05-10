using Microsoft.EntityFrameworkCore;
using RealEstate.API.Data;
using RealEstate.API.Models;

namespace RealEstate.API.Repository.Database
{
    public class PropertyDBRepository : IPropertyRepository
    {
        private readonly RealEDbContext _dbContext;

        public PropertyDBRepository(RealEDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EstateProperty?> AddProperty(EstateProperty newproperty)
        {
            await _dbContext.AddAsync(newproperty);
            await _dbContext.SaveChangesAsync();

            return newproperty;
        }

        public Task DeleteProperty(int propertyId)
        {
            var getproperty = this._dbContext.EstateProperties.FindAsync(propertyId);
            if (getproperty.Result != null)
            {
                this._dbContext.EstateProperties.Remove(getproperty.Result);
            }


            return this._dbContext.SaveChangesAsync();

        }

        public async Task<List<EstateProperty>> GetAllProperty()
        {
            return await _dbContext.EstateProperties.ToListAsync();
        }

        public Task<EstateProperty> GetPropertyById(int Id)
        {
            var getproperty = this._dbContext.EstateProperties
                     .FirstOrDefaultAsync(m => m.Id == Id);

            if (getproperty == null)
            {
                return null;
            }


            return getproperty;
        }

        public EstateProperty UpdateProperty(int propertyId, EstateProperty newproperty)
        {
            _dbContext.Update(newproperty);
            _dbContext.SaveChanges();
            return newproperty;
        }
    }
}

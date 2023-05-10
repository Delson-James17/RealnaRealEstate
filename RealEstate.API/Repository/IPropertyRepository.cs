using RealEstate.API.Models;

namespace RealEstate.API.Repository
{
    public interface IPropertyRepository
    {

        Task<List<EstateProperty>> GetAllProperty();

        Task<EstateProperty?> GetPropertyById(int Id);

        Task<EstateProperty?> AddProperty(EstateProperty newproperty);

        EstateProperty UpdateProperty(int propertyId, EstateProperty newproperty);

        Task DeleteProperty(int propertyId);

    }
}

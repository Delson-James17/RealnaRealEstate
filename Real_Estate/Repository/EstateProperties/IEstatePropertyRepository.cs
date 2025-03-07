using Real_Estate.Models;
using Real_Estate.ViewModels;

namespace Real_Estate.Repository.EstateProperties
{
    public interface IEstatePropertyRepository
    {
        Task<EstateProperty> AddEstateProperty(string userId, CreatePropertyViewModel newEstateProperty);
        Task<EstateProperty> UpdateEstateProperty(int estatePropertyId, EditPropertyViewModel estatePropertyToBeUpdated);
        Task<EstateProperty> DeleteEstateProperty(int estatePropertyId);
        Task<int> GetEstatePropertiesCount();
        Task<EstateProperty> GetEstatePropertyById(int estatePropertyId);
        Task<EstateProperty> GetEstatePropertyWithRelatedEntitiesById(int estatePropertyId);
        Task<EstateProperty> GetEstatePropertyWithUserById(int estatePropertyId);
        Task<List<EstateProperty>> GetAllEstateProperties();
        Task<List<EstateProperty>> GetEstatePropertiesByPropertyCategory(int propertyCategoryId);
        Task<List<EstateProperty>> GetEstatePropertiesBySearchString(string searchString, int? saleOrRentModelId, int? propertyCategoryId);
    }
}

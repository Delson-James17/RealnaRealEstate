using RealEstate.API.Models;

namespace RealEstate.API.Repository
{
    public interface IPropertyCategoryRepository
    {
        Task<List<PropertyCategory>> GetAllPropertyCategory();

        Task<PropertyCategory?> GetPropertyCategoryById(int Id);

        Task<PropertyCategory?> AddPropertyCategory(PropertyCategory newcategory);

        PropertyCategory UpdatePropertyCategory(int propertyId, PropertyCategory newcategory);

        Task DeletePropertyCategory(int propertyId);
    }
}

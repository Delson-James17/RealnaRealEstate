using Microsoft.AspNetCore.Identity;
using Real_Estate.Models;

namespace Real_Estate.Repository.PropertyCategoryRepositories
{
    public interface IPropertyCategoryRepository
    {
        Task<List<PropertyCategory>> GetAllPropertyCategories();
        Task<List<PropertyCategory>> GetAllPropertyCategoriesNames();
        Task<PropertyCategory> GetPropertyCategoryById(int propertyCategoryId);
        Task<PropertyCategory> AddPropertyCategory(PropertyCategory newPropertyCategory);
        Task<PropertyCategory> UpdatePropertyCategory(int propertyCategoryId, PropertyCategory propertyCategoryToUpdate);
        Task<PropertyCategory> DeletePropertyCategoryById(int propertyCategoryId);
    }
}

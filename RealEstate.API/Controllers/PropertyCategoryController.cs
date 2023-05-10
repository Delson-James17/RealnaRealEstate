using Microsoft.AspNetCore.Mvc;
using RealEstate.API.DTO;
using RealEstate.API.Models;
using RealEstate.API.Repository;
using RealEstate.API.Repository.Database;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyCategoryController : ControllerBase
    {

        private readonly IPropertyCategoryRepository _propertyCategoryRepository;
        public PropertyCategoryController(IPropertyCategoryRepository propertyCategoryRepository)
        {
            _propertyCategoryRepository = propertyCategoryRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProperytyCategories()
        {
            return Ok(await _propertyCategoryRepository.GetAllPropertyCategory());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPropertyCategoryById(int id)
        {
            var property = await _propertyCategoryRepository.GetPropertyCategoryById(id);

            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePropertyCategory([FromBody] PropertyCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newPropertyCat = new PropertyCategory
            {
                Name = dto.Name,
                Description = dto.Description,
            };

            var property = await _propertyCategoryRepository.AddPropertyCategory(newPropertyCat);

            return Ok(property);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProperty(int id)
        {
            var property = await _propertyCategoryRepository.GetPropertyCategoryById(id);

            if (property == null)
            {
                return NotFound();
            }

            await _propertyCategoryRepository.DeletePropertyCategory(property.Id);

            return Ok(property);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> EditEstateProperty(int id, PropertyCategoryDto dto)
        {
            var property = await _propertyCategoryRepository.GetPropertyCategoryById(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (property == null)
            {
                return NotFound();
            }

            property.Name = dto.Name;
            property.Description = dto.Description;

            var propertyToReturn = _propertyCategoryRepository.UpdatePropertyCategory(id, property);

            return Ok(propertyToReturn);
        }

    }
}

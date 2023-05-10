using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate.API.DTO;
using RealEstate.API.Models;
using RealEstate.API.Repository;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstatePropertiesController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPropertyRepository _propertyRepository;
        public EstatePropertiesController(IPropertyRepository propertyRepository, IUserRepository userRepository)
        {
            _propertyRepository = propertyRepository;
            _userRepository = userRepository;
        }
        

        [HttpGet]
        [Authorize ]
        public async Task<IActionResult> GetAllEstateProperties()
        {
           return Ok(await _propertyRepository.GetAllProperty());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPropertyById(int id)
        {
            var property = await _propertyRepository.GetPropertyById(id);

            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEstateProperty([FromBody] EstatePropertyDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getcurrentUser = await _userRepository.GetCurrentUser();
            var newProperty = new EstateProperty
            {
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address,
                UrlImages = dto.UrlImages,
                Price = dto.Price,
                OwnerName = dto.OwnerName,
                SaleOrRentModelId = dto.SaleOrRentModelId,
                PropertyCategoryId = dto.PropertyCategoryId,
                ApplicationUserId = getcurrentUser.Id
                
            };

            var property = await _propertyRepository.AddProperty(newProperty);

            return Ok(property);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProperty(int id)
        {
            var property = await _propertyRepository.GetPropertyById(id);

            if (property == null)
            {
                return NotFound();
            }

            await _propertyRepository.DeleteProperty(property.Id);

            return Ok(property);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> EditEstateProperty(int id, EstatePropertyDto dto)
        {
            var property = await _propertyRepository.GetPropertyById(id);
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
            property.Address = dto.Address;
            property.UrlImages = dto.UrlImages;
            property.Price = dto.Price;
            property.OwnerName = dto.OwnerName;
            property.SaleOrRentModelId = dto.SaleOrRentModelId;
            property.PropertyCategoryId = dto.PropertyCategoryId;


            var propertyToReturn = _propertyRepository.UpdateProperty(id, property);

            return Ok(propertyToReturn);
        }

    }
}

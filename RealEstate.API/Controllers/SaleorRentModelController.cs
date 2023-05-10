using Microsoft.AspNetCore.Mvc;
using RealEstate.API.DTO;
using RealEstate.API.Models;
using RealEstate.API.Repository;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleorRentModelController : ControllerBase
    {

        private readonly ISaleorRentRepository _saleorRentDBRepository;
        public SaleorRentModelController(ISaleorRentRepository saleorRentDBRepository )
        {
            _saleorRentDBRepository = saleorRentDBRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSaleorRentCategories()
        {
            return Ok(await _saleorRentDBRepository.GetSaleorRentCategory());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPropertyCategoryById(int id)
        {
            var property = await _saleorRentDBRepository.GetSaleorRentCategoryById(id);

            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        [HttpPost]
        public async Task<IActionResult>CreateSaleorRentCategories([FromBody] SaleorRentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newPropertyCat = new SaleorRentModel
            {
                Name = dto.Name,
            };

            var property = await _saleorRentDBRepository.AddSaleorRentCategory(newPropertyCat);

            return Ok(property);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSaleorRent(int id)
        {
            var property = await _saleorRentDBRepository.GetSaleorRentCategoryById(id);

            if (property == null)
            {
                return NotFound();
            }

            await _saleorRentDBRepository.DeleteSaleorRentCategory(property.Id);

            return Ok(property);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> EditSaleorRent([FromRoute] int id, [FromBody] SaleorRentDto dto)
        {
            var property = await _saleorRentDBRepository.GetSaleorRentCategoryById(id);//

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (property == null)
            {
                return NotFound();
            }

            //var updatedProperty = new SaleorRentModel//
            //{
            //    Name = dto.Name,
            //};


            property.Name = dto.Name;

            var propertyToReturn = _saleorRentDBRepository.UpdateSaleorRentCategory(id, property);

            return Ok(propertyToReturn);
        }
    }
}

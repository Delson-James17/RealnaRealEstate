using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstate.API.Data;
using RealEstate.API.DTO;
using RealEstate.API.Models;
using RealEstate.API.Repository;
using System.Security.Claims;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly RealEDbContext _context;

        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {

            return Ok(await _appointmentRepository.GetAllAppointment());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAppointmentById( int id)
        {
        
            var appointment = await _appointmentRepository.GetAppointmentById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }
       /* [HttpPost]
        public IActionResult CreateAppointment(AppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser? client = _context.ApplicationUsers.Find(clientId);
            var newAppointment = new Appointment
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                OwnerId = dto.OwnerId,
                OwnerScheduleId = dto.OwnerScheduleId,
                EstatePropertyId = dto.EstatePropertyId
            };
            var appointment = _appointmentRepository.AddAppointment(newAppointment);
            return Ok(appointment);
        }*/
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProperty(int id)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            await _appointmentRepository.DeleteAppointment(appointment.Id);

            return Ok(appointment);
        }

      /*  [HttpPost("{id}")]
        public IActionResult EditEstateProperty(int id, AppointmentDto dto)
        {
            var appointment = _appointmentRepository.GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newAppointment = new Appointment
            {
                Id = dto.Id,
                ClientId = dto.ClientId,
                OwnerId = dto.OwnerId,
                OwnerScheduleId = dto.OwnerScheduleId,
                EstatePropertyId = dto.EstatePropertyId
            };
            var appointmentToReturn = _appointmentRepository.AddAppointment(newAppointment);
            return Ok(appointmentToReturn);

        }*/
    }

}

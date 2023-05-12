using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Real_Estate.Repository.Appointments;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.ViewModels;
using System.Security.Claims;
using Real_Estate.Repository.OwnerSchedules;
using Real_Estate.Repository.EstateProperties;

namespace Real_Estate.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IOwnerScheduleRepository _ownerScheduleRepository;
        private readonly IEstatePropertyRepository _estatePropertyRepository;
        private RealEDbContext _context;

        public AppointmentController(UserManager<ApplicationUser> userManager, 
                                        RealEDbContext context, 
                                        IAppointmentRepository appointmentRepository, 
                                        IOwnerScheduleRepository ownerScheduleRepository,
                                        IEstatePropertyRepository estatePropertyRepository)
        {
            _userManager = userManager;
            _context = context;
            this._appointmentRepository = appointmentRepository;
            this._ownerScheduleRepository = ownerScheduleRepository;
            this._estatePropertyRepository = estatePropertyRepository;
        }

        public IActionResult DeleteConfirmation()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            string currentlyLoggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser? currentlyLoggedInUser = await _context.ApplicationUsers.FindAsync(currentlyLoggedInUserId);
            var currentlyLoggedInUserRoles = await _userManager.GetRolesAsync(currentlyLoggedInUser);

            var appointmentList = await this._appointmentRepository.GetAppointmentsByIdAndUserRole(
                currentlyLoggedInUser.Id,
                currentlyLoggedInUserRoles.First()
            );

            return View(appointmentList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAppointment(int ownerScheduleId, int propertyId)
        {
            OwnerSchedule ownerSchedule = await this._ownerScheduleRepository
                .GetOwnerScheduleById(ownerScheduleId);

            EstateProperty estateProperty = await this._estatePropertyRepository
                .GetEstatePropertyById(propertyId);

            CreateAppointmentViewModel createAppointmentViewModel = new CreateAppointmentViewModel()
            {
                OwnerScheduleId = ownerScheduleId,
                EstatePropertyId = propertyId,
                OwnerSchedule = ownerSchedule,
                EstateProperty = estateProperty
            };

            return View(createAppointmentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(CreateAppointmentViewModel appointment)
        {
            if (!ModelState.IsValid)
            {
                return View(appointment);
            }

            string clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            EstateProperty property = await this._estatePropertyRepository
                .GetEstatePropertyById(appointment.EstatePropertyId);
            ApplicationUser? client = await _context.ApplicationUsers.FindAsync(clientId);

            await this._appointmentRepository.AddAppointment(client.Id, property.ApplicationUserId, appointment);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetPropertyAppointments(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await this._estatePropertyRepository.GetEstatePropertyWithUserById(id);

            if (property == null)
            {
                return NotFound();
            }

            var ownerSchedules = await this._ownerScheduleRepository
                .GetOwnerScheduleByPropertyIdAndOwnerId(id, property.ApplicationUserId);

            return View(ownerSchedules);
        }

        [HttpPost]
        public async Task<IActionResult> GetPropertyAppointments(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                await this._appointmentRepository.AddAppointment(appointment);
                return RedirectToAction(nameof(GetPropertyAppointments), new { id = appointment.EstatePropertyId });
            }
            return View(appointment);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Appointment appointment = await this._appointmentRepository.GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (appointment.ClientId != userId && appointment.OwnerId != userId)
            {
                return Forbid();
            }

            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this._appointmentRepository.DeleteAppointmentById(id);
            return RedirectToAction("Index");
        }
    }
}

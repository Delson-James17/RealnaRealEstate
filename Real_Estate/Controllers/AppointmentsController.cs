using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.ViewModels;
using System.Security.AccessControl;
using System.Security.Claims;

namespace Real_Estate.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private RealEDbContext _context;
        public AppointmentController(UserManager<ApplicationUser> userManager, RealEDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult DeleteConfirmation()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            string clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Find user in db using its id
            ApplicationUser? client = await _context.ApplicationUsers.FindAsync(clientId);
            var userRoles = await _userManager.GetRolesAsync(client);
            var appointmentQuery = _context.Appointments
                .Include(ep => ep.EstateProperty)
                .Include(os => os.OwnerSchedules)
                .AsQueryable();

            if (userRoles.First() == "Owner")
            {
                appointmentQuery = appointmentQuery.Where(u => u.OwnerId == client.Id);
            }
           else 
            {
                appointmentQuery = appointmentQuery.Where(u => u.ClientId == client.Id);
            }

            appointmentQuery = appointmentQuery.Join(_userManager.Users, a => a.ClientId, u => u.Id, (a, u) => new { a, u })
                .Select(ap => new Appointment
                {
                    Id = ap.a.Id,
                    OwnerSchedules = ap.a.OwnerSchedules,
                    Clients = ap.u,
                    EstateProperty = ap.a.EstateProperty,
                    EstatePropertyId = ap.a.EstatePropertyId,
                });

            var appointmentList = await appointmentQuery.ToListAsync();   
            return View(appointmentList);
        }


        [HttpGet]
        public async Task<IActionResult> CreateAppointment(int ownerScheduleId, int propertyId)
        {

            /* var clients = _userManager.GetUsersInRoleAsync("Client").Result.Select(u => new { u.Id, u.UserName });
             var owners = _userManager.GetUsersInRoleAsync("Owner").Result.Select(u => new { u.Id, u.UserName });
             ViewBag.ClientId = new SelectList(clients, "Id", "UserName");
             ViewBag.OwnerId = new SelectList(owners, "Id", "UserName");
             ViewBag.OwnerScheduleId = new SelectList(_context.OwnerSchedules, "Id", "Name");
             ViewBag.PropertyId = new SelectList(_context.EstateProperties, "Id", "Name");
            */

            OwnerSchedule ownerSchedule = await this._context.OwnerSchedules
                .Where(os => os.Id == ownerScheduleId)
                .FirstOrDefaultAsync();

            EstateProperty estateProperty = await this._context.EstateProperties
                .Where(ep => ep.Id == propertyId)
                .FirstOrDefaultAsync();

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

            // Get currently logged in client
            string clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Find user in db using its id
            ApplicationUser? client = await _context.ApplicationUsers.FindAsync(clientId);
            EstateProperty property = await _context.EstateProperties.Where(ep => ep.Id == appointment.EstatePropertyId).FirstOrDefaultAsync();

            Appointment newAppointment = new Appointment
            {
                ClientId = client.Id,
                OwnerId = property.ApplicationUserId,
                OwnerScheduleId = appointment.OwnerScheduleId,
                EstatePropertyId = appointment.EstatePropertyId,
            };

            await this._context.Appointments.AddAsync(newAppointment);

            await this._context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> GetPropertyAppointments(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var property = await _context.EstateProperties.Include(a => a.ApplicationUser).FirstOrDefaultAsync(a => a.Id == id);

            if (property == null)
            {
                return NotFound();
            }


            //.Where(a => a.OwnerId == property.ApplicationUser.Id)
            //.Select(o => new EventViewModel
            //{
            //    Url = $"/Appointment/CreateAppointment?ownerScheduleId={o.Id}&propertyId={id}",
            //    Title = $"{o.startTime.Value.ToShortTimeString()} - {o.endTime.Value.ToShortTimeString()}",
            //    Start = o.startTime.ToString(),
            //    End = o.endTime.ToString(),
            //})
            //.ToListAsync();

            //.Join(_context.Appointments,
            //        ownerSchedule => ownerSchedule.Id,
            //        appointment => appointment.ClientId,
            //        (ownerSchedule, appointment) => new
            //        {
            //            Id = ownerSchedule.Id,
            //            OwnerId = ownerSchedule.OwnerId,
            //            Owner = ownerSchedule.Owner
            //        });

            //var ownerSchedules = await _context.OwnerSchedules
            //    .Where(a => a.OwnerId == property.ApplicationUser.Id)
            //    .Join(_context.Appointments,
            //        os => os.Id,
            //        a => a.OwnerScheduleId,
            //        (os, a) => new { os, a })
            //    .Where(osa => osa.os.Id )
            //    .ToListAsync();

            var ownerScheduleQuery =
                from ownerSchedule in _context.OwnerSchedules
                where ownerSchedule.OwnerId == property.ApplicationUser.Id
                where !(from appointment in _context.Appointments
                        select appointment.OwnerScheduleId).Contains(ownerSchedule.Id)
                select new EventViewModel
                {
                    Url = $"/Appointment/CreateAppointment?ownerScheduleId={ownerSchedule.Id}&propertyId={id}",
                    Title = $"{ownerSchedule.startTime} - {ownerSchedule.endTime}",
                    Start = ownerSchedule.startTime.ToString(),
                    End = ownerSchedule.endTime.ToString()
                };

            var ownerSchedules = await ownerScheduleQuery.ToListAsync();

            return View(ownerSchedules);
        }
        [HttpPost]
        public async Task<IActionResult> GetPropertyAppointments(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetPropertyAppointments), new { id = appointment.EstatePropertyId });
            }
            return View(appointment);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Appointment appointment = _context.Appointments.Find(id);

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
            Appointment appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}

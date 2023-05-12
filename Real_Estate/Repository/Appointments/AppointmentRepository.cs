using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.ViewModels;

namespace Real_Estate.Repository.Appointments
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly RealEDbContext _realEDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentRepository(RealEDbContext realEDbContext, UserManager<ApplicationUser> userManager)
        {
            this._realEDbContext = realEDbContext;
            this._userManager = userManager;
        }

        public async Task<List<Appointment>> GetAppointmentsByIdAndUserRole(string userId, string userRole)
        {
            var appointmentQuery = this._realEDbContext.Appointments
                .Include(ep => ep.EstateProperty)
                .Include(os => os.OwnerSchedules)
                .AsQueryable();

            if (userRole == "Owner")
            {
                appointmentQuery = appointmentQuery.Where(u => u.OwnerId == userId);
            }
            else
            {
                appointmentQuery = appointmentQuery.Where(u => u.ClientId == userId);
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

            return await appointmentQuery.ToListAsync();
        }

        public async Task<int> GetAppointmentsCount()
        {
            return await this._realEDbContext.Appointments.CountAsync();
        }

        public async Task<List<Appointment>> GetUserAppointment(string userId, bool isOwner = false)
        {
            throw new NotImplementedException();
        }

        public async Task<Appointment> AddAppointment(Appointment newAppointment)
        {
            await this._realEDbContext.Appointments.AddAsync(newAppointment);
            await this._realEDbContext.SaveChangesAsync();

            return newAppointment;
        }

        public async Task<Appointment> AddAppointment(string clientId, string ownerId, CreateAppointmentViewModel createAppointmentViewModel)
        {
            Appointment newAppointment = new Appointment
            {
                ClientId = clientId,
                OwnerId = ownerId,
                OwnerScheduleId = createAppointmentViewModel.OwnerScheduleId,
                EstatePropertyId = createAppointmentViewModel.EstatePropertyId,
            };

            await this._realEDbContext.Appointments.AddAsync(newAppointment);
            await this._realEDbContext.SaveChangesAsync();

            return newAppointment;
        }

        public async Task<Appointment> GetAppointmentById(int appointmentId)
        {
            Appointment? appointment = await this._realEDbContext.Appointments
                .Where(appointment => appointment.Id == appointmentId)
                .FirstOrDefaultAsync();
                
            return appointment;
        }

        public async Task<Appointment> DeleteAppointmentById(int appointmentId)
        {
            Appointment appointment = await this._realEDbContext.Appointments.FindAsync(appointmentId);

            if (appointment != null)
            {
                this._realEDbContext.Appointments.Remove(appointment);
                await this._realEDbContext.SaveChangesAsync();

                return appointment;
            }

            return null;
        }
    }
}

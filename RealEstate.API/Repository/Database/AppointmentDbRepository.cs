using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstate.API.Data;
using RealEstate.API.Models;

namespace RealEstate.API.Repository.Database
{
    public class AppointmentDbRepository : IAppointmentRepository
    {
        private readonly RealEDbContext _dbContext;
        public AppointmentDbRepository(RealEDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Appointment AddAppointment(Appointment newappointment)
        {
            _dbContext.AddAsync(newappointment);
            _dbContext.SaveChangesAsync();

            return newappointment;
        }

        public Task DeleteAppointment(int appointmentId)
        {
            var getappointment = this._dbContext.Appointments.FindAsync(appointmentId);
            if (getappointment.Result != null)
            {
                this._dbContext.Appointments.Remove(getappointment.Result);
            }


            return this._dbContext.SaveChangesAsync();
        }

        public Task<List<Appointment>> GetAllAppointment()
        {
            return this._dbContext.Appointments.ToListAsync();
        }

        public Task<Appointment> GetAppointmentById(int Id)
        {
            var getappointment = this._dbContext.Appointments
                     .FirstOrDefaultAsync(m => m.Id == Id);

            if (getappointment == null)
            {
                return null;
            }

            return getappointment;
        }

        public Appointment UpdateAppointment(int appointmentId, Appointment newappointment)
        {
            _dbContext.Update(newappointment);
            _dbContext.SaveChanges();
            return newappointment;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.ViewModels;

namespace Real_Estate.Repository.OwnerSchedules
{
    public class OwnerScheduleRepository : IOwnerScheduleRepository
    {
        private readonly RealEDbContext _realEDbContext;

        public OwnerScheduleRepository(RealEDbContext realEDbContext)
        {
            this._realEDbContext = realEDbContext;
        }

        public async Task<OwnerSchedule> AddOwnerSchedule(OwnerSchedule newOwnerSchedule)
        {
            await this._realEDbContext.OwnerSchedules.AddAsync(newOwnerSchedule);
            await this._realEDbContext.SaveChangesAsync();

            return newOwnerSchedule;
        }

        public async Task<OwnerSchedule> DeleteOwnerScheduleById(int ownerScheduleId)
        {
            OwnerSchedule? ownerSchedule = this._realEDbContext.OwnerSchedules.FirstOrDefault(s => s.Id == ownerScheduleId);

            if(ownerSchedule != null)
            {
                this._realEDbContext.OwnerSchedules.Remove(ownerSchedule);
                await this._realEDbContext.SaveChangesAsync();

                return ownerSchedule;
            }

            return null;
        }

        public async Task<OwnerSchedule> GetOwnerScheduleById(int ownerScheduleId)
        {
            var ownerSchedule = await this._realEDbContext.OwnerSchedules
                .FirstOrDefaultAsync(ownerSchedule => ownerSchedule.Id == ownerScheduleId);
                
            return ownerSchedule ?? new OwnerSchedule();
        }

        public async Task<List<EventViewModel>> GetOwnerScheduleByPropertyIdAndOwnerId(int propertyId, string properOwnerId)
        {
            var ownerScheduleQuery =
                from ownerSchedule in this._realEDbContext.OwnerSchedules
                where ownerSchedule.OwnerId == properOwnerId
                where !(from appointment in this._realEDbContext.Appointments
                        select appointment.OwnerScheduleId).Contains(ownerSchedule.Id)
                select new EventViewModel
                {
                    Url = $"/Appointment/CreateAppointment?ownerScheduleId={ownerSchedule.Id}&propertyId={propertyId}",
                    Title = $"{ownerSchedule.startTime} - {ownerSchedule.endTime}",
                    Start = ownerSchedule.startTime.ToString(),
                    End = ownerSchedule.endTime.ToString()
                };

            var ownerSchedules = await ownerScheduleQuery.ToListAsync();

            return ownerSchedules;
        }

        public async Task<List<OwnerSchedule>> GetOwnerScheduleList(string? ownerId)
        {
            var ownerSchedulesQuery = this._realEDbContext.OwnerSchedules.AsQueryable<OwnerSchedule>();

            if(ownerId != null)
            {
                ownerSchedulesQuery = ownerSchedulesQuery
                    .Where(ownerSchedule => ownerSchedule.OwnerId == ownerId);
            }

            return await ownerSchedulesQuery.ToListAsync();
        }

        public async Task<List<EventViewModel>> GetOwnerSchedulesByPropertyId()
        {
            throw new NotImplementedException();
        }

        public async Task<OwnerSchedule> UpdateOwnerSchedule(OwnerSchedule ownerScheduleToBeUpdated)
        {
            OwnerSchedule? oldSchedule = await this._realEDbContext.OwnerSchedules
                .FirstOrDefaultAsync(s => s.Id == ownerScheduleToBeUpdated.Id);

            if(oldSchedule != null)
            {
                oldSchedule.startTime = ownerScheduleToBeUpdated.startTime;
                oldSchedule.endTime = ownerScheduleToBeUpdated.endTime;

                this._realEDbContext.OwnerSchedules.Update(oldSchedule);
                this._realEDbContext.SaveChanges();

                return ownerScheduleToBeUpdated;
            }

            return null;
        }
    }
}

using Real_Estate.Models;
using Real_Estate.ViewModels;

namespace Real_Estate.Repository.OwnerSchedules
{
    public interface IOwnerScheduleRepository
    {
        Task<OwnerSchedule> AddOwnerSchedule(OwnerSchedule newOwnerSchedule);
        Task<OwnerSchedule> UpdateOwnerSchedule(OwnerSchedule ownerScheduleToBeUpdated);
        Task<OwnerSchedule> DeleteOwnerScheduleById(int ownerScheduleId);
        Task<List<EventViewModel>> GetOwnerSchedulesByPropertyId();
        Task<OwnerSchedule> GetOwnerScheduleById(int ownerScheduleId);
        Task<List<OwnerSchedule>> GetOwnerScheduleList(string ownerId);
        Task<List<EventViewModel>> GetOwnerScheduleByPropertyIdAndOwnerId(int propertyId, string properOwnerId);
    }
}

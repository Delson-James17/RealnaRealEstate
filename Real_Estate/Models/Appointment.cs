using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Real_Estate.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        [ValidateNever]
        public ApplicationUser? Clients { get; set; }
        public string OwnerId { get; set; }
        [ValidateNever]
        public ApplicationUser? Owners { get; set; }
        public int OwnerScheduleId { get; set; }
        [ValidateNever]
        public OwnerSchedule? OwnerSchedules { get; set; }
        public int EstatePropertyId { get; set; }

        [ValidateNever]
        public EstateProperty? EstateProperty { get; set; }
        //authit trail
        public DateTime? CreationDate { get; set; } = DateTime.Now;
    }
}

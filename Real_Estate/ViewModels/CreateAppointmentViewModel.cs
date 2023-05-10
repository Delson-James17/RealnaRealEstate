using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Real_Estate.Models;
using System.ComponentModel.DataAnnotations;

namespace Real_Estate.ViewModels
{
    public class CreateAppointmentViewModel
    {
        [Required]
        public int OwnerScheduleId { get; set; }

        [Required]
        public int EstatePropertyId { get; set; }

        [ValidateNever]
        public OwnerSchedule OwnerSchedule { get; set; }

        [ValidateNever]
        public EstateProperty EstateProperty { get; set; }
    }
}

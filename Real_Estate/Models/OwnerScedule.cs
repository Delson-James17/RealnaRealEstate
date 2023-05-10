using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Real_Estate.Models
{
    public class OwnerSchedule
    {
        public int Id { get; set; }

        public string? OwnerId { get; set; }

        [ValidateNever]
        public ApplicationUser? Owner { get; set; }

        [BindProperty, DataType(DataType.Time)]
        [Required]
        public DateTime? startTime { get; set; }

        [BindProperty, DataType(DataType.Time)]
        [Required]
        public DateTime? endTime { get; set; }
    }
}

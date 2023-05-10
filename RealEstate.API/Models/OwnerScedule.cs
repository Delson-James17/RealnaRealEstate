using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RealEstate.API.Models;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.API.Models
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

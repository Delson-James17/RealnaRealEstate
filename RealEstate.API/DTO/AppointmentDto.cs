using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.API.DTO
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string OwnerId { get; set; }
        public int OwnerScheduleId { get; set; }
        public int EstatePropertyId { get; set; }
        //authit trail
        public DateTime? CreationDate { get; set; } = DateTime.Now;
    }
}

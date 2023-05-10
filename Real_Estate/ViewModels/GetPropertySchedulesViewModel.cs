using Real_Estate.Models;

namespace Real_Estate.ViewModels
{
    public class GetPropertySchedulesViewModel
    {
        List<OwnerSchedule> OwnerSchedules { get; set; }
        public List<EventModel> Events { get; set; }
        public class EventModel
        {
            public string Start { get; set; }
            public string End { get; set; }
            public bool AllDay { get; set; } = true;
        }
    }
}

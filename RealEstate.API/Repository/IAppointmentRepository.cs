using RealEstate.API.Models;
using System.Threading.Tasks;

namespace RealEstate.API.Repository
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAllAppointment();

        Task<Appointment?> GetAppointmentById(int Id);

        Appointment AddAppointment(Appointment newappointment);

        Appointment UpdateAppointment(int appointmentId, Appointment newappointment);

        Task DeleteAppointment(int appointmentId);
    }
}

using Real_Estate.Models;
using Real_Estate.ViewModels;

namespace Real_Estate.Repository.Appointments
{
    public interface IAppointmentRepository
    {
        Task<int> GetAppointmentsCount();
        Task<List<Appointment>> GetUserAppointment(string userId, bool isOwner = false);
        Task<Appointment> AddAppointment(Appointment newAppointment);
        Task<Appointment> AddAppointment(string clientId, string ownerId, CreateAppointmentViewModel createAppointmentViewModel);
        Task<Appointment> DeleteAppointmentById(int appointmentId);
        Task<List<Appointment>> GetAppointmentsByIdAndUserRole(string userId, string userRole);
        Task<Appointment> GetAppointmentById(int appointmentId);
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.Repository.Appointments;
using Real_Estate.Repository.EstateProperties;
using Real_Estate.ViewModels;

namespace Real_Estate.Controllers
{
    public class AdminController : Controller
    {
        private readonly RealEDbContext _context;
        private readonly IEstatePropertyRepository _estatePropertyRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(RealEDbContext context, 
                                IEstatePropertyRepository estatePropertyRepository, 
                                IAppointmentRepository appointmentRepository, 
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this._estatePropertyRepository = estatePropertyRepository;
            this._appointmentRepository = appointmentRepository;
            this._userManager = userManager;
        }
        public IActionResult AdminPannel()
        {
            return View();
        }

        public async Task<IActionResult> RowCount()
        {
            int propertyCount = await this._estatePropertyRepository.GetEstatePropertiesCount();
            int appointmentCount = await this._appointmentRepository.GetAppointmentsCount();
            int userCount = await this._userManager.Users.CountAsync();

            var model = new RowCountViewModel
            {
                PropertyCount = propertyCount,
                AppointmentCount = appointmentCount,
                UserCount = userCount
            };

            List<EstateProperty> properties = await this._estatePropertyRepository.GetAllEstateProperties();
            AdminDashboardViewModel adminDashboardViewModel = new AdminDashboardViewModel()
            {
                EstateProperties = properties,
                RowCount = model
            };
            return View(adminDashboardViewModel);
        }

        public IActionResult NotificationBell()
        {
            var model = new RowCountViewModel
            {

            };

            return View(model);
        }
    }
}

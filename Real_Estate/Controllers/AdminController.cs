using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.ViewModels;

namespace Real_Estate.Controllers
{
    public class AdminController : Controller
    {
        private readonly RealEDbContext _context;

        public AdminController(RealEDbContext context)
        {
            _context = context;
        }
        public IActionResult AdminPannel()
        {
            return View();
        }
        public async Task<IActionResult> RowCount()
        {
            var model = new RowCountViewModel
            {
                PropertyCount = _context.EstateProperties.Count(),
                AppointmentCount = _context.Appointments.Count(),
                UserCount = _context.ApplicationUsers.Count()
            };
            List<EstateProperty> properties = await _context.EstateProperties.ToListAsync();
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

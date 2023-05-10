using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Real_Estate.Data;
using Real_Estate.Models;


namespace Real_Estate.Controllers
{
    public class OwnerScheduleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RealEDbContext _dbContext;

        public OwnerScheduleController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            RealEDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }
        public IActionResult AppointmentComplete()
        {
            return View();
        }

        public IActionResult UpdateScheduleComplete()
        {
            return View();
        }

        public IActionResult OwnerScheduleList()
        {
            if (User.IsInRole("Owner"))
            {
                var ownerId = _userManager.GetUserId(User);
                var ownerSchedules = _dbContext.OwnerSchedules.Where(s => s.OwnerId == ownerId).ToList();
                return View(ownerSchedules);
            }

            var schedules = _dbContext.OwnerSchedules.ToList();
            return View(schedules);
        }
        [HttpGet]
        public IActionResult AddSchedule()
        {
            if (User.IsInRole("Owner"))
            {
                ViewBag.MyValue = _userManager.GetUserId(User);
                return View();
            }

            OwnerSchedule schedule = new OwnerSchedule();
            return View(schedule);
        }

        [HttpPost]
        public IActionResult AddSchedule(OwnerSchedule sched)
        {
            string ownerId = _userManager.GetUserId(User);
            ViewBag.MyValue = ownerId;
            var ownerSchedules = _dbContext.OwnerSchedules
                .Where(s => s.OwnerId == ownerId)
                .ToList();
            sched.OwnerId = ownerId;

            if (sched.startTime.HasValue && sched.endTime.HasValue && sched.startTime.Value >= sched.endTime.Value)
            {
                ModelState.AddModelError("startTime", "Start time must be before the end time.");
            }
            if (!ModelState.IsValid)
            {
                return View(sched);
            }

            _dbContext.OwnerSchedules.Add(sched);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(AppointmentComplete));
        }
        [HttpGet]
        public IActionResult UpdateSchedule(int id)
        {
            OwnerSchedule schedule = _dbContext.OwnerSchedules.FirstOrDefault(s => s.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }
            return View(schedule);
        }

        [HttpPost]
        public IActionResult UpdateSchedule(OwnerSchedule updatedSchedule)
        {
            OwnerSchedule oldSchedule = _dbContext.OwnerSchedules.FirstOrDefault(s => s.Id == updatedSchedule.Id);
            if (oldSchedule == null)
            {
                return NotFound();
            }

           // oldSchedule.dayOfWeek = updatedSchedule.dayOfWeek;
            oldSchedule.startTime = updatedSchedule.startTime;
            oldSchedule.endTime = updatedSchedule.endTime;

            _dbContext.SaveChanges();

           // return RedirectToAction(nameof(OwnerScheduleList));
            return RedirectToAction("UpdateScheduleComplete", "OwnerSchedule");
        }
        public IActionResult DeleteSchedule(int id)
        {
            var schedule = _dbContext.OwnerSchedules.FirstOrDefault(s => s.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            _dbContext.OwnerSchedules.Remove(schedule);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(OwnerScheduleList));
        }

    }
}
    


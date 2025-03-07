using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Real_Estate.Data;
using Real_Estate.Models;
using Real_Estate.Repository.OwnerSchedules;

namespace Real_Estate.Controllers
{
    public class OwnerScheduleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RealEDbContext _dbContext;
        private readonly IOwnerScheduleRepository _ownerScheduleRepository;

        public OwnerScheduleController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            RealEDbContext dbContext,
            IOwnerScheduleRepository ownerScheduleRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            this._ownerScheduleRepository = ownerScheduleRepository;
        }

        public IActionResult AppointmentComplete()
        {
            return View();
        }

        public IActionResult UpdateScheduleComplete()
        {
            return View();
        }

        public async Task<IActionResult> OwnerScheduleList()
        {
            string? ownerId = User.IsInRole("Owner") ? this._userManager.GetUserId(User) : null;

            var ownerSchedules = await this._ownerScheduleRepository.GetOwnerScheduleList(ownerId);

            return View(ownerSchedules);
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
        public async Task<IActionResult> AddSchedule(OwnerSchedule sched)
        {
            string ownerId = _userManager.GetUserId(User);
            var ownerSchedules = this._ownerScheduleRepository.GetOwnerScheduleList(ownerId);

            ViewBag.MyValue = ownerId;
            sched.OwnerId = ownerId;

            if (sched.startTime.Value >= sched.endTime.Value)
            {
                ModelState.AddModelError("startTime", "Start time must be before the end time.");
                return View(sched);
            }

            if (!ModelState.IsValid)
            {
                return View(sched);
            }

            await this._ownerScheduleRepository.AddOwnerSchedule(sched);

            return RedirectToAction(nameof(AppointmentComplete));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSchedule(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OwnerSchedule schedule = await this._ownerScheduleRepository.GetOwnerScheduleById(id);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSchedule(OwnerSchedule updatedSchedule)
        {
            if(!ModelState.IsValid)
            {
                return View(updatedSchedule);
            }

            await this._ownerScheduleRepository.UpdateOwnerSchedule(updatedSchedule);

            return RedirectToAction("UpdateScheduleComplete", "OwnerSchedule");
        }

        public async Task<IActionResult> DeleteSchedule(int id)
        {
            if(id == null)
            {
                return NotFound();
            }

            await this._ownerScheduleRepository.DeleteOwnerScheduleById(id);
            return RedirectToAction(nameof(OwnerScheduleList));
        }
    }
}
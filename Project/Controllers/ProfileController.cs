using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.DataBase;
using Project.Identity;
using Project.ViewModels;

namespace Project.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Context context;

        public ProfileController(Context _context, UserManager<ApplicationUser> _userManager)
        {
            userManager = _userManager;
            context = _context;
        }
        public async Task<IActionResult> Index(string username)
        {
            ApplicationUser? user = await userManager.FindByNameAsync(username);
            if (user.Role == "Mentor")
            {
                return RedirectToAction("MentorProfile", user);
            }
            else
            {
                return RedirectToAction("StudentProfile", user);
            }
        }
        public IActionResult MentorProfile(ApplicationUser user)
        {
            MentorViewModel MVM = new MentorViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Requests = context.Requests.Where(r => r.MentorId == user.Id).ToList(),
                AboutMe = user.Description
            };
            return View(MVM);
        }
        public IActionResult StudentProfile(ApplicationUser user)
        {
            StudentViewModel MVM = new StudentViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Requests = context.Requests.Where(r => r.StudentId == user.Id).ToList(),
            };
            return View(MVM);
        }
        //Edit??
    }
}

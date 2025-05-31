using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DataBase;
using Project.Identity;
using Project.Models;
using Project.ViewModels;
using System.Security.Claims;

namespace Project.Controllers
{
    public class MentorController : Controller
    {
        private readonly Context context;
        private readonly UserManager<ApplicationUser> userManager;

        public MentorController(Context _context, UserManager<ApplicationUser> userManager)
        {
            context = _context;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var Mentors = context.Users.Where(u => u.Role == "Mentor").ToList();
            List<AllMentors> All = new List<AllMentors>();
            foreach (var Mentor in Mentors)
            {
                AllMentors temp = new AllMentors()
                {
                    Description = Mentor.Description,
                    UserName = Mentor.UserName,
                };
                All.Add(temp);
            }
            return View(All);
        }
        [HttpGet]
        public IActionResult SendReport()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendReport(ReportViewModel reportviewmodel)
        {
            Report report = new Report()
            {
                Type = reportviewmodel.Type,
                Content = reportviewmodel.Content
            };
            context.Reports.Add(report);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SendRequest(string mentorusername)
        {
            ApplicationUser? student = await userManager.FindByNameAsync(User.Identity.Name);
            ApplicationUser? mentor = await userManager.FindByNameAsync(mentorusername);
            Request request = new Request()
            {
                StudentId = student.Id,
                MentorId = mentor.Id,
            };
            var check = await context.Requests.Where(c => c.MentorId == request.MentorId && c.StudentId == request.StudentId).FirstOrDefaultAsync();

            if (check == null)
            {
                context.Requests.Add(request);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AcceptRequest(string mentorname, string studentname)
        {
            Console.WriteLine(mentorname);
            Console.WriteLine(studentname);
            ApplicationUser? Mentor = await userManager.FindByNameAsync(mentorname);
            ApplicationUser? Student = await userManager.FindByNameAsync(studentname);
            var cur = await context.Requests.FirstOrDefaultAsync(r => r.MentorId == Mentor.Id && r.StudentId == Student.Id);
            cur.Active = true;
            context.SaveChanges();
            return RedirectToAction("Index", "Profile",new { username = mentorname });
        }
        //public async Task<IActionResult> DeleteRequest()
        //{
        //    ApplicationUser? Any = await userManager.FindByNameAsync(User.Identity.Name);

        //    var request = await context.Requests.FirstOrDefaultAsync(r => r.MentorId == Any.Id || r.StudentId == Any.Id);

        //    context.Requests.Remove(request);
        //    context.SaveChanges();
        //    ApplicationUser? Mentor = await userManager.FindByNameAsync(User.Identity.Name);
        //    return RedirectToAction("Index", "Profile");
        //}
    }
}

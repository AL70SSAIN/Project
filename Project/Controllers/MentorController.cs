using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DataBase;
using Project.Models;
using Project.ViewModels;
using System.Security.Claims;

namespace Project.Controllers
{
    public class MentorController : Controller
    {
        private readonly Context context;
        public MentorController(Context _context)
        {
            context = _context;
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
        [HttpPost]
        [Authorize(Roles = "Student")]
        public IActionResult SendReport(ReportViewModel reportviewmodel)
        {
            Report report = new Report()
            {
                StudentId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                MentorId = reportviewmodel.MentorId,
                Type = reportviewmodel.Type,
                Content = reportviewmodel.Content
            };
            context.Reports.Add(report);
            context.SaveChanges();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Student")]
        public IActionResult SendRequest(RequestViewModel reportviewmodel)
        {
            Request request = new Request()
            {
                StudentId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                MentorId = reportviewmodel.MentorId,
                Message = reportviewmodel.Message
            };
            context.Requests.Add(request);
            context.SaveChanges();
            return View();
        }
    }
}

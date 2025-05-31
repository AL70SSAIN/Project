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
    public class RoadMapController : Controller
    {
        private readonly Context context;
        private readonly UserManager<ApplicationUser> userManager;
        public RoadMapController(Context _context, UserManager<ApplicationUser> userManager)
        {
            context = _context;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var RoadMaps = context.RoadMaps.Include(r => r.Bloger).ToList();
            return View(RoadMaps);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Mentor")]
        public IActionResult AddRoadMaps()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Mentor")]
        public async Task<IActionResult> AddRoadMaps(RoadMapViewModel roadmap)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? usermodel = await userManager.FindByNameAsync(User.Identity.Name);
                if (usermodel != null)
                {
                    RoadMap newroadmap = new RoadMap()
                    {
                        Name = roadmap.Name,
                        Link = roadmap.Link,
                        Description = roadmap.Description,
                        BlogerId = usermodel.Id,
                        Date = DateOnly.FromDateTime(DateTime.Now)
                    };
                    context.RoadMaps.Add(newroadmap);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "You can't add a Road Map");

                }
            }
            return View(roadmap);
        }
        [Authorize]
        [Authorize(Roles = "Admin,Mentor")]
        public async Task<IActionResult> DeleteRoadMaps(int roadmapid)
        {
            var del = context.RoadMaps.FirstOrDefault(r => r.Id == roadmapid);
            ApplicationUser? usermodel = await userManager.FindByNameAsync(User.Identity.Name);
            if (del != null && usermodel != null && del.BlogerId == usermodel.Id)
            {
                context.RoadMaps.Remove(del);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}

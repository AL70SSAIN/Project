using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DataBase;
using Project.Models;
using Project.ViewModels;
using System.Security.Claims;

namespace Project.Controllers
{
    public class RoadMapController : Controller
    {
        private readonly Context context;
        public RoadMapController(Context _context)
        {
            context = _context;
        }
        public IActionResult Index()
        {
            var RoadMaps = context.RoadMaps.Include(r=>r.Bloger).ToList();
            return View(RoadMaps);
        }
        [HttpGet]
        [Authorize(Roles = "Mentor")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRoadMaps()
        {
            Console.WriteLine("LOL1");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Mentor")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRoadMaps(RoadMapViewModel roadmap)
        {
            Console.WriteLine("LOL2");
            if (ModelState.IsValid)
            {
                RoadMap newroadmap = new RoadMap()
                {
                    Name = roadmap.Name,
                    Link = roadmap.Link,
                    Description = roadmap.Description,
                    BlogerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                };
                context.RoadMaps.Add(newroadmap);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roadmap);
        }
        [Authorize]
        [Authorize(Roles = "Mentor")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteRoadMaps(int roadmapid)
        {
            var del = context.RoadMaps.FirstOrDefault(r => r.Id == roadmapid);
            if (del != null)
            {
                context.RoadMaps.Remove(del);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}

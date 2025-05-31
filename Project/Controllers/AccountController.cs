using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Identity;
using Project.Models;
using Project.ViewModels;

namespace Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signinmanager;
        public AccountController(
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signinmanager)
        {
            userManager = _userManager;
            signinmanager = _signinmanager;
        }
        [HttpGet]
        public IActionResult RegisterStudent()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterStudent(RegisterStudentViewModel NewUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = NewUser.UserName;
                user.Email = NewUser.Email;
                user.PhoneNumber = NewUser.PhoneNumber;
                user.Role = "Student";
                ApplicationUser? usermodel = await userManager.FindByEmailAsync(user.Email);
                if (usermodel == null)
                {
                    IdentityResult result = await userManager.CreateAsync(user, NewUser.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Student");
                        await signinmanager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "This Email is already used");
                }
            }
            return View(NewUser);
        }
        [HttpGet]
        public IActionResult RegisterMentor()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterMentor(RegisterMentorViewModel NewUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = NewUser.UserName;
                user.Email = NewUser.Email;
                user.PhoneNumber = NewUser.PhoneNumber;
                user.Description = NewUser.Description;
                user.Role = "Mentor";
                ApplicationUser? usermodel = await userManager.FindByEmailAsync(user.Email);
                if (usermodel == null)
                {
                    IdentityResult result = await userManager.CreateAsync(user, NewUser.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Mentor");
                        await signinmanager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "This Email is already used");
                }
            }
            return View(NewUser);
        }
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInUserViewModel User)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? usermodel = await userManager.FindByEmailAsync(User.Email);
                if (usermodel != null)
                {
                    bool found = await userManager.CheckPasswordAsync(usermodel, User.Password);
                    if (found)
                    {
                        await signinmanager.SignInAsync(usermodel, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Email and Password doesn't match");
            }
            return View(User);
        }
        public async Task<IActionResult> LogOut()
        {
            await signinmanager.SignOutAsync();
            return RedirectToAction("LogIn", "Account");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

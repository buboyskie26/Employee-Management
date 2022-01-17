using HrMan.Data;
using HrMan.Models;
using HrMan.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HrMan.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _web;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;
        private readonly string role;
        public AccountController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<Employee> userManager,
            RoleManager<IdentityRole> roleManager, IWebHostEnvironment web, SignInManager<Employee> signInManager)
        {
           
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _web = web;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }


        [HttpPost]
        public async Task<IActionResult> LogOff(LoginVM model)
        {
            var user =  _userManager.GetUserAsync(User).Result;

            user.Login = false;
            user.DateLogOff = DateTime.Now;
            await _userManager.UpdateAsync(user);

            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {

                var obj = new Employee
                {
                   
                    Login = true
                };

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {

                    var user = await _userManager.FindByNameAsync(model.Email);
                    user.Login = true;
                    user.LoginCount += 1;
                    user.DateLogin = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                    HttpContext.Session.SetString("ssuserName", user.Firstname);
                    //var userName = HttpContext.Session.GetString("ssuserName");
                   
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt");
            }
            return View(model);
        }


        public IActionResult Login()
        {

            return View();
        }
        public async Task<IActionResult> Register()
        {
            if (!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Helper.SuperVisor));
                await _roleManager.CreateAsync(new IdentityRole(Helper.Manager));
                await _roleManager.CreateAsync(new IdentityRole(Helper.Regular));
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM Input)
        {
            if (ModelState.IsValid)
            {
                var user = new Employee
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Lastname = Input.LastName,
                    Firstname = Input.FirstName,
                    Fullname = Input.Fullname,
                    contactNumber = Input.contactNumber,
                    newUser = 1,
                    DateOfBirth = Input.Birthday,
                    DateJoined = Input.JoinedDate,
                    Role = Input.RoleName,
                    Login = true,
                    DateLogin = DateTime.Now
                };
                if (Input.ImageUrl.Length > 0 && Input.ImageUrl != null)
                {
                    var imageRoute = @"images/employees";
                    var fileName = Path.GetFileNameWithoutExtension(Input.ImageUrl.FileName);
                    var extension = Path.GetExtension(Input.ImageUrl.FileName);
                    var webroot = _web.WebRootPath;

                    fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;

                    var path = Path.Combine(webroot, imageRoute, fileName);

                    await Input.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    user.ImageUrl = "/" + imageRoute + "/" + fileName;

                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Input.RoleName);
                    if (!User.IsInRole(Helper.Admin))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    else
                    {
                        TempData["newAdmin"] = user.Firstname;
                    }
                    return RedirectToAction("Index", "LeaveAllocation");
                }


            }
            return View();
        }

    }

}

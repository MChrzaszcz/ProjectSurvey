using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjectSurvey.Data;
using ProjectSurvey.Models;
using ProjectSurvey.Models.AccountViewModels;
using ProjectSurvey.Models.SurveyUserViewModel;



namespace ProjectSurvey.Controllers
{
//    [Authorize]
    public class SurveyUserController : Controller
    {
        private readonly UserManager<SurveyUser> _userManager;
        private readonly SignInManager<SurveyUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //        private static bool _databaseChecked;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;


        public SurveyUserController(UserManager<SurveyUser> userManager, SignInManager<SurveyUser> signInManager,
            ILoggerFactory loggerFactory, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<SurveyUserController>();
            _context = context;
        }
//        [Authorize(Policy = "CanViewUserList")]
        public IActionResult UserList(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            var users = from s in _context.SurveyUsers
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    users = users.OrderBy(s => s.FirstName);
                    break;
                case "date_desc":
                   users = users.OrderByDescending(s => s.LastName);
                    break;
                default:
                    users = users.OrderBy(s => s.LastName);
                    break;
            }
         

            return View(users.ToList());
        }


        public IActionResult UserChart(string userId)
            {
            var userSpheres = _context.UserSpheres.Include(s => s.Result);
            List<double> years = new List<double>();
            List<double> months = new List<double>();
            List<double> days = new List<double>();
            List<double> levels = new List<double>();

//            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (var sphere in userSpheres)
            {
//                dataPoints.Add(new DataPoint(sphere.CompleteSphereDate, sphere.Result.Level));

                years.Add(sphere.CompleteSphereDate.Year);
                months.Add(sphere.CompleteSphereDate.Month);
                days.Add(sphere.CompleteSphereDate.Day);
                levels.Add(sphere.Result.Level);
            }
            ViewBag.DateYear = JsonConvert.SerializeObject(years);
            ViewBag.DateMonth = JsonConvert.SerializeObject(months);
            ViewBag.DateDay = JsonConvert.SerializeObject(days);
            ViewBag.Levels = JsonConvert.SerializeObject(levels);

//            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }


        public IActionResult Index()
        {
            return View(_context.SurveyUsers);
        }

        public IActionResult Register()
        {

            return View(new RegistrationViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {

                if (ModelState.IsValid)
                {
                    var user = new SurveyUser() { UserName = model.Email,
                        LastName = model.LastName,
                        FirstName = model.FirstName,
                        Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);


                    if (result.Succeeded)
                    {
                    if (!_roleManager.RoleExistsAsync("NormalUser").Result)
                    {
                        IdentityRole role = new IdentityRole();
                        role.Name = "NormalUser";
//                        role.Description = "Perform normal operations.";
                        IdentityResult roleResult = _roleManager.
                        CreateAsync(role).Result;
                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError("",
                             "Error while creating role!");
                            return View();
                        }
                    }
                    if (!_roleManager.RoleExistsAsync("SuperUser").Result)
                    {
                        IdentityRole role = new IdentityRole();
                        role.Name = "SuperUser";
                        //                        role.Description = "Perform normal operations.";
                        IdentityResult roleResult = _roleManager.
                        CreateAsync(role).Result;
                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError("",
                             "Error while creating role!");
                            return View();
                        }
                    }

                    _userManager.AddToRoleAsync(user,
                         "SuperUser").Wait();


                    await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(3, "User created a new account with password.");
                         return RedirectToAction(nameof(HomeController.Index), "Home");
            
                }              
                }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ProjectSurvey.Models.SurveyUserViewModel.LoginViewModel model)
        {
//            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
//                if (result.RequiresTwoFactor)
//                {
//                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
//                }
//                if (result.IsLockedOut)
//                {
//                    _logger.LogWarning(2, "User account locked out.");
//                    return View("Lockout");
//                }
               
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);              
            }



            // If we got this far, something failed, redisplay form
            return View(model);
        }


        
        public IActionResult Login()
        {
            return View(new ProjectSurvey.Models.SurveyUserViewModel.LoginViewModel());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }
}
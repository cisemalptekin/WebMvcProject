using Microsoft.AspNetCore.Mvc;
using WebMvcProject.Data;
using WebMvcProject.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebMvcProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ProjectDbContext _databaseContext;
        private readonly IConfiguration _configuration;

        public AccountController(ProjectDbContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                User user = _databaseContext.Users.ToList().FirstOrDefault(x => x.UserName.ToLower() == model.UserName.ToLower() && BCrypt.Net.BCrypt.Verify(model.Password, hashedPassword));

                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError(nameof(model.UserName), " User is locked");
                        return View(model);
                    }

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.Name ?? string.Empty));
                    claims.Add(new Claim(ClaimTypes.Surname, user.Surname ?? string.Empty));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                    claims.Add(new Claim("Username", user.UserName));

                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is incorrect");
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(x => x.UserName.ToLower() == model.UserName.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.UserName), "Username is already exists");
                    View(model);
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                User user = new User()
                {
                    Name = model.Name,
                    Surname = model.SurName,
                    UserName = model.UserName,
                    Email = model.EmailAddress,
                    Password = hashedPassword
                };

                _databaseContext.Users.Add(user);
                _databaseContext.Students.Add(new Student() { Name = user.Name, Surname = user.Surname, PhoneNumber = model.PhoneNumber});

                int affectedRowCount = _databaseContext.SaveChanges();

                if (affectedRowCount == 0)
                {
                    ModelState.AddModelError("", "User can not be added");
                }
                else
                {
                    return RedirectToAction(nameof(Login));
                }
            }

            return View(model);
        }

        public IActionResult Profile()
        {
            Guid UserID = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _databaseContext.Users.SingleOrDefault(x => x.Id == UserID);

            ViewData["UserName"] = user.UserName;
            return View();
        }

        [HttpPost]
        public IActionResult ProfileChangeUserName([Required][StringLength(50)] string? username)
        {
            if (ModelState.IsValid)
            {
                Guid UserID = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == UserID);

                user.UserName = username;
                _databaseContext.SaveChanges();
                return RedirectToAction(nameof(Profile));
            }
            return View("Profile");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

    }
}

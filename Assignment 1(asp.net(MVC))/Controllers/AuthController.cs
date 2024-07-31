using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Assignment_1_asp.net_MVC__.Models;
using System.Security.Cryptography;
using System.Text;

namespace Assignment_1_asp.net_MVC__.Controllers
{
    public class AuthController : Controller
    {
        private readonly FoodContext db;
        public AuthController(FoodContext _db)
        {
            db = _db;
        }

        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(User user)
        {
            var CheckExistingUser = db.Users.FirstOrDefault(t => t.Email == user.Email);
            if (CheckExistingUser != null)
            {
                ViewBag.msg = "User Already Exists";
                return View();
            }

            var hasher = new PasswordHasher<string>();
            var userToCreate = new User
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password = hasher.HashPassword(user.Email, user.Password),
                RoleId = 2,
                Status = 0
            };
            db.Users.Add(userToCreate);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {

            bool IsAuthenticated = false;
            string controller = "";

            ClaimsIdentity identity = null;

            var checkUser = db.Users.FirstOrDefault(u => u.Email == user.Email);
            if (checkUser != null)
            {
                var hasher = new PasswordHasher<string>();
                var verifyPass = hasher.VerifyHashedPassword(checkUser.Email, checkUser.Password, user.Password);

                if (verifyPass == PasswordVerificationResult.Success && checkUser.RoleId == 1)
                {
                    identity = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Name ,checkUser.Username),
                    new Claim(ClaimTypes.Role ,"Admin"),
                }
                   , CookieAuthenticationDefaults.AuthenticationScheme);
                    IsAuthenticated = true;
                    controller = "Admin";
                }
                else if (verifyPass == PasswordVerificationResult.Success && checkUser.RoleId == 2)
                {
                    IsAuthenticated = true;
                    identity = new ClaimsIdentity(new[]
                   {
                    new Claim(ClaimTypes.Name ,checkUser.Username),
                    new Claim(ClaimTypes.Role ,"User"),
                }
                   , CookieAuthenticationDefaults.AuthenticationScheme);
                    controller = "Home";
                }
                else
                {
                    IsAuthenticated = false;
                    ViewBag.msg = "Invalid Credentials";

                }
                if (IsAuthenticated)
                {
                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", controller);
                }

                else
                {

                    return View();
                }




            }
            else
            {
                ViewBag.msg = "User not found";
                return View();
            }


        }
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


    }
}

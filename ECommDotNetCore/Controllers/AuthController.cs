using ECommDotNetCore.Data;
using ECommDotNetCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ECommDotNetCore.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext db;
        public AuthController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [AcceptVerbs("Post","Get")]
        public IActionResult CheckExistingEmailId(string email)
        {
            var data = db.user.Where(x=>x.Email == email).SingleOrDefault();
            if(data!=null)
            {
                return Json($"Email {email} Already in Use");
            }
            else
            {
                return Json(true);
            }
        }

        public static string EncryptedPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] pass = ASCIIEncoding.ASCII.GetBytes(password);
                string encrpass = Convert.ToBase64String(pass);
                return encrpass;
            }
        }

        public static string DecryptedPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] pass = Convert.FromBase64String(password);
                string decrpass = ASCIIEncoding.ASCII.GetString(pass);
                return decrpass;
            }
        }

        [HttpPost]
        public IActionResult SignUp(User u)
        {
            u.Password = EncryptedPassword(u.Password);
            u.Role = "User";
            db.user.Add(u);
            db.SaveChanges();
            return RedirectToAction("SignIn");
        }

        public IActionResult AllUser()
        {
            var data = db.user.ToList();
            return Json(data);
        }

        public IActionResult SignIn()
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult SignIn(LoginModel log)
        {
            var data = db.user.Where(x => x.Email.Equals(log.Email)).SingleOrDefault();
            if (data != null)
            {
                bool us = data.Email.Equals(log.Email) && DecryptedPassword(data.Password).Equals(log.Password);
                if (us)
                {
                    HttpContext.Session.SetString("myuser",data.Email);
                    HttpContext.Session.SetString("urole",data.Role);
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    TempData["ErrorPassword"] = "Invalid Password";
                }
            }
            else
            {
                TempData["ErrorEmail"] = "Invalid Email";
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn");
        }
    }
}

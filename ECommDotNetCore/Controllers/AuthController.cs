using ECommDotNetCore.Data;
using ECommDotNetCore.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public IActionResult SignUp(User u)
        {
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
        public IActionResult SignIn(User u)
        {
            var data = db.user.Where(x => x.Email == u.Email && x.Password == u.Password);
            if (data != null)
            {
                return RedirectToAction("SignUp");
            }
            else
            {
                return View();
            }
        }        
    }
}

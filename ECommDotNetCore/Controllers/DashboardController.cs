using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ECommDotNetCore.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var suser = HttpContext.Session.GetString("myuser");
            if(suser == null)
            {
                return RedirectToAction ("SignIn", "Auth");
            }
            return View();
        }
    }
}

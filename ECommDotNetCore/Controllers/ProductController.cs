using ECommDotNetCore.Data;
using ECommDotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ECommDotNetCore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext db;
        private IWebHostEnvironment env;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public IActionResult Index(string choice)
        {
            if (choice == "All")
            {
                var data = db.products.Take(5).ToList();
                return View(data);
            }
            else
            {
                var data = db.products.OrderBy(x => x.Price).ToList();
                return View(data);
            }
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(ProductViewModel pm)
        {
            var path = env.WebRootPath;
            var filePath = "Content/Images/" + pm.Picture.FileName;
            var fullPath = Path.Combine(path, filePath);
            UploadFile(pm.Picture, fullPath);
            var obj = new Product()
            {
                Pname = pm.Pname,
                Pcat = pm.Pcat,
                Price = pm.Price,
                Picture = filePath
            };
            db.Add(obj);
            db.SaveChanges();
            TempData["msg"] = "Product Added Successfully!!!";
            return RedirectToAction("Index");
        }

        public void UploadFile(IFormFile file, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
        }

        public IActionResult LTH()
        {
            var data = db.products.OrderBy(x => x.Price).ToList();
            return View(data);
        }

        public IActionResult AddToCart(int id)
        {
            string sess = HttpContext.Session.GetString("myuser");
            var prod = db.products.Find(id);
            var obj = new Cart()
            {
                Pname = prod.Pname,
                Pcat = prod.Pcat,
                Picture = prod.Picture,
                Price = prod.Price,
                Suser = sess
            };
            db.carts.Add(obj);
            db.SaveChanges();
            return RedirectToAction("ShowCart");
        }

        public IActionResult ShowCart()
        {
            if(HttpContext.Session.GetString("myuser").IsNullOrEmpty())
            {
                return RedirectToAction("SignIn", "Auth");
            }
            else
            {
                var sess = HttpContext.Session.GetString("myuser");
                var prod = db.carts.Where(x=>x.Suser == sess).ToList();
                return View(prod);
            }
        }
    }
}

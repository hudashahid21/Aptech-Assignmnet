using Assignment_1_asp.net_MVC__.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Assignment_1_asp.net_MVC__.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodContext db;
        public HomeController(FoodContext _db)
        {
            db = _db;
        }
        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
            return View();

        }

        [Authorize(Roles = "User")]

        public IActionResult About()
        {
            return View();
        }
        [Authorize(Roles = "User")]

        public IActionResult ContactUs()
        {
            return View();
        }
        [Authorize(Roles = "User")]

        public IActionResult Book()
        {
            return View();
        }
        [Authorize(Roles = "User")]

        public IActionResult Menu()
        {
            var productsData = db.Products.Include(a => a.Category);
            foreach (var item in productsData)
            {
                item.Description = item.Description.Length > 50 ? item.Description.Substring(0, 80)+".." : item.Description;
            }
            return View(productsData);
        }
        [Authorize(Roles = "User")]
        public IActionResult Details(int id)
        {
            var ItemsData = db.Products.Include(a => a.Category);
            var ItemDetail = ItemsData.Where(b => b.ProductId == id).ToList();
            if (ItemDetail != null)
            {

                return View(ItemDetail);
            }
            else
            {
                return RedirectToAction("Menu");
            }
        }
    }
}

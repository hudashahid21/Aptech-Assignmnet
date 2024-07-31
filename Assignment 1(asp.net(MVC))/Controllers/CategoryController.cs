using Assignment_1_asp.net_MVC__.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment_1_asp.net_MVC__.Controllers
{
    public class CategoryController : Controller
    {
        private readonly FoodContext db;

        public CategoryController(FoodContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            var catData = db.Categories.ToList();
            return View(catData);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var category = db.Categories.FirstOrDefault(x => x.CategoryId == id);

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Category category)
        {
            if (true)
            {
                db.Categories.Update(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Category/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {

            var category = db.Categories.FirstOrDefault(x => x.CategoryId == id);

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = db.Categories.FirstOrDefault(x => x.CategoryId == id);

            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

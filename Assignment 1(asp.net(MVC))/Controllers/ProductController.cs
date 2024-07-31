using Assignment_1_asp.net_MVC__.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Assignment_1_asp.net_MVC__.Controllers
{
    public class ProductController : Controller
    {
        private readonly FoodContext db;
        public ProductController(FoodContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var productsData = db.Products.Include(a => a.Category);
            return View(productsData);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product prod, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Ensure the Uploads folder exists
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate a unique image name
                string imageName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                string imagePath = Path.Combine(uploadsFolder, imageName);

                // Save the image file
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Set the image URL for storing in the database
                string dbImageUrl = Path.Combine("/Uploads", imageName).Replace("\\", "/");
                prod.Image = dbImageUrl;
            }

            db.Products.Add(prod);
            db.SaveChanges();

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item1 = db.Products.Find(id);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View(item1);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product prod, IFormFile file, string oldimage)
        {
            var dbimage = "";
            if (file != null && file.Length > 0)
            {
                var imageName = DateTime.Now.ToString("yymmddhhmmss") + Path.GetFileName(file.FileName);
                string imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");
                var imagevalue = Path.Combine(imagepath, imageName);
                using (var stream = new FileStream(imagevalue, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                dbimage = Path.Combine("/Uploads", imageName);
                prod.Image = dbimage;
            }
            else
            {
                // Retain old image if no new file is uploaded
                prod.Image = oldimage;
            }

            // Retrieve the existing product from the database
            var itemInDb = db.Products.FirstOrDefault(x => x.ProductId == prod.ProductId);
            if (itemInDb != null)
            {
                // Update product properties
                itemInDb.Name = prod.Name;
                itemInDb.Price = prod.Price;
                itemInDb.Description = prod.Description;
                itemInDb.Image = prod.Image;
                itemInDb.CategoryId = prod.CategoryId; // Explicitly set the CategoryId

                db.Entry(itemInDb).State = EntityState.Modified;
                db.SaveChanges();
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", prod.CategoryId);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var product = db.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return RedirectToAction("Index");
        }

    }
}

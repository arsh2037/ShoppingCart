using Bulky.DataAccess.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db) {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList= _db.Categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category objCategory)
        {
            if (objCategory.Name==objCategory.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Display Order and Name cannot be same");
            }
            if (objCategory.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "Test is invalid value");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(objCategory);
                _db.SaveChanges();
                 return RedirectToAction("Index","Category");
            }
            else
            {
                return View();
            }
        }
    }
}

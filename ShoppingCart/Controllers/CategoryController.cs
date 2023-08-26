using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Edit(int? CategoryId)
        {
            if (CategoryId == null || CategoryId == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _categoryRepo.Get(u=>u.CategoryId==CategoryId);
            //Category? categoryFromDb1 = _categoryRepo.Categories.FirstOrDefault(u=>u.Id==CategoryId);
            //Category? categoryFromDb2 = _categoryRepo.Categories.Where(u=>u.Id==CategoryId).FirstOrDefault();

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Delete(int? CategoryId)
        {
            if (CategoryId == null || CategoryId == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _categoryRepo.Get(u => u.CategoryId == CategoryId);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? CategoryId)
        {
            Category? categoryFromDb = _categoryRepo.Get(u => u.CategoryId == CategoryId);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            _categoryRepo.Remove(categoryFromDb);
            _categoryRepo.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
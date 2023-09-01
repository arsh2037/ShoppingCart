// Importing necessary namespaces
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Utilities;

// Define the namespace for the CategoryController under the Admin area
namespace ECommerceWeb.Areas.Admin.Controllers
{
    // Specify the controller's area as "Admin" and restrict access to only those with the "Admin" role
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        // Dependency for unit of work (database operations)
        private readonly IUnitOfWork _unitOfWork;

        // Constructor to initialize the unit of work using Dependency Injection
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Action to display a list of all categories
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        // Action to render the category creation view
        public IActionResult Create()
        {
            return View();
        }

        // Action to handle the POST request of category creation
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            // Check if the Name and DisplayOrder properties are identical
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }

            // If the model is valid, add the category to the database
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        // Action to render the category edit view
        public IActionResult Edit(int? CategoryId)
        {
            if (CategoryId == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryId == CategoryId);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        // Action to handle the POST request of category editing
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        // Action to render the category deletion confirmation view
        public IActionResult Delete(int? CategoryId)
        {
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryId == CategoryId);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        // Action to handle the POST request of category deletion
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? CategoryId)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.CategoryId == CategoryId);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

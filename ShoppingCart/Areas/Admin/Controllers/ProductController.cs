using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Bulky.DataAccess.Data;
namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Edit(int? Id)
        {
            if (Id == 0)
            {
                return NotFound();
            }
            Product? ProductFromDb = _unitOfWork.Product.Get(u => u.Id == Id);
            //Product? ProductFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==Id);
            //Product? ProductFromDb2 = _db.Categories.Where(u=>u.Id==Id).FirstOrDefault();

            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Delete(int? Id)
        {

            Product? ProductFromDb = _unitOfWork.Product.Get(u => u.Id == Id);

            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? Id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == Id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
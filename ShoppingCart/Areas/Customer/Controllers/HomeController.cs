// Importing necessary namespaces
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

// Define the namespace for the HomeController
namespace ECommerceWeb.Areas.Customer.Controllers
{
    // Specify the controller's area and attribute it to "Customer"
    [Area("Customer")]
    public class HomeController : Controller
    {
        // Dependencies for logging and unit of work (database operations)
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        // Constructor to initialize dependencies using Dependency Injection
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // Action to show products. If an ID (product name fragment) is provided, filters products by name
        public IActionResult Index(string? id = null)
        {
            if (id != null)
            {
                IEnumerable<Product> productList = _unitOfWork.Product.GetAll(s => s.Title.Contains(id), includeProperties: "Category");
                return View(productList);
            }
            else
            {
                IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
                return View(productList);
            }
        }

        // Action to display details of a specific product based on its ID
        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }

        // Action to handle product addition to the shopping cart
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            // Retrieve the logged-in user's ID
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            // Check if the product is already in the cart for the user
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingCart.ProductId);

            if (cartFromDb != null)
            {
                // Update the quantity if the product already exists in the cart
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            else
            {
                // Add the product to the cart if it's a new addition
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }

            // Notify the user of the successful cart update
            TempData["Success"] = "Cart updated Sucessfully";

            // Save changes to the database
            _unitOfWork.Save();

            // Redirect to the products listing page
            return RedirectToAction(nameof(Index));
        }

        // Privacy view action 
        public IActionResult Privacy()
        {
            return View();
        }

        // Error action to handle and display errors
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

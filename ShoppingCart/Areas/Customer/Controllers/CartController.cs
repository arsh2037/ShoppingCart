// Import necessary namespaces
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models.ViewModels;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;

// Define the namespace for the CartController
namespace ECommerceWeb.Areas.Customer.Controllers
{
    // Define the area for this controller and require authorization for access
    [Area("customer")]
    [Authorize]
    public class CartController : Controller
    {
        // Dependency injection for the Unit of Work and ShoppingCart view model
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        // Constructor initializes the Unit of Work object
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Action to show the cart items for the logged-in user
        public IActionResult Index()
        {
            // Get the current user's ID
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Retrieve the shopping cart items for the current user
            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product")
            };

            // Calculate the price and total order amount for each item in the cart
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.Count);
            }

            // Return the view with the shopping cart view model
            return View(ShoppingCartVM);
        }

        // Action to show a summary of the cart items for the logged-in user
        public IActionResult Summary()
        {
            // This method is similar to the Index action and could potentially be refactored to avoid code duplication

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product")
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        // Action to increment the quantity of a cart item
        public IActionResult Plus(int cartId)
        {
            // Retrieve the cart item from the database
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            // Increment the count for the cart item
            cartFromDb.Count += 1;

            // Update the cart item in the database
            _unitOfWork.ShoppingCart.Update(cartFromDb);

            // Save changes to the database
            _unitOfWork.Save();

            // Redirect to the cart Index view
            return RedirectToAction(nameof(Index));
        }

        // Action to decrement the quantity of a cart item
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            // Check if the count is 1 or less
            if (cartFromDb.Count <= 1)
            {
                // Remove the cart item from the database
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                // Decrement the count for the cart item
                cartFromDb.Count -= 1;

                // Update the cart item in the database
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // Action to remove a cart item completely
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            // Remove the cart item from the database
            _unitOfWork.ShoppingCart.Remove(cartFromDb);

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // Helper method to get the price based on the quantity
        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }
    }
}

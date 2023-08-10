using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Models
{
    public class Products : Controller
    {
        public IActionResult Index()
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            return View();
        }
    }
}

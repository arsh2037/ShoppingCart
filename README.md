# ShoppingCart
The Udemy Course that I followed throughout the HIT339 Unit to understand .NET  in a comprehensive manner along with Microsoft docs: https://www.udemy.com/course/complete-aspnet-core-21-course/learn/ 
Shopping Cart is a project using C# and ASP.NET Core MvC architecture.

This project was made using a variety of online resources available (StackOverflow, MS docs, ChatGPT(Debugging), and the Udemy Course I followed)

The project uses N-tier architecture and a Repository pattern. 
Role Based Authorization (RBAC) is implemented, which means no user with a customer role can access the content management page that allows the deletion of products from the site CRUD operations on categories. Users with customer roles can only purchase the product, edit the cart, or remove the product form the cart.

To set up the Project locally:
Go to ECommerce -> Appsettings.json:
  "DefaultConnection": "Server={Your Local Server Name};Database={Your Database Name};Trusted_Connection=True;TrustServerCertificate=True"

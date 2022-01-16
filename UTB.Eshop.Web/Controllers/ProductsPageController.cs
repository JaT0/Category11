using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTB.Eshop.Web.Controllers;
using UTB.Eshop.Web.Models.Database;
using UTB.Eshop.Web.Models.Entity;
using UTB.Eshop.Web.Models.ViewModels;


namespace Tatyrkova.Eshop.Web.Controllers
{
    public class ProductsPageController : Controller
    {

        readonly EshopDbContext eshopDbContext;
        IWebHostEnvironment env;

        public ProductsPageController(EshopDbContext eshopDB, IWebHostEnvironment env)
        {
            eshopDbContext = eshopDB;
            this.env = env;
        }


        public string OnPost(string categoryName)
        {
            return categoryName;

        }

        public IList<Product> Categories(string categoryName)
        {
            IList<Product> product = (IList<Product>)eshopDbContext.Products
                .Where(product => product.Category == categoryName).ToList();
            return (product);
           
        }

        public async Task<IActionResult> Category(string categoryName,
            int? pageNumber
            ,string currentCategory)
        {


            // DbSet<Product> products = eshopDbContext.Products;

            //ProductsPageViewModel productsVM = new ProductsPageViewModel();

            if (categoryName != null)
            {
                pageNumber = 1;
            }
            else
            {
                categoryName = currentCategory;
            }

            
            var product = eshopDbContext.Products
                        .Where(proCat => proCat.Category == categoryName);
            

            //IList<Product> product = (IList<Product>)eshopDbContext.Products
            //    .Where(product => product.Category == categoryName).ToList();
            //productsVM.Products = product;

            int pageSize = 3;

            return View(  await PaginatedList<Product>.CreateAsync(product.AsNoTracking(), pageNumber ?? 1, pageSize));
        }



    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTB.Eshop.Web.Models.Database;
using UTB.Eshop.Web.Models.Entity;

namespace UTB.Eshop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        readonly EshopDbContext eshopDbContext;

        public ProductController(ILogger<ProductController> logger, EshopDbContext eshopDb)
        {
            _logger = logger;
            eshopDbContext = eshopDb;
        }

        public IActionResult Detail(int ID)
        {
            Product produkt = eshopDbContext.Products.FirstOrDefault(prod => prod.ID == ID);

            if (produkt != null)
            {
                return View(produkt);
            }

            return NotFound();
        }
    }
}

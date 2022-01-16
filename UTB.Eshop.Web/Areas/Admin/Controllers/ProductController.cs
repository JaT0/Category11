using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTB.Eshop.Web.Models.Database;
using UTB.Eshop.Web.Models.Entity;
using UTB.Eshop.Web.Models.Identity;
using UTB.Eshop.Web.Models.Implementation;

namespace UTB.Eshop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin) + ", " + nameof(Roles.Manager))]
    public class ProductController : Controller
    {
        readonly EshopDbContext eshopDbContext;
        readonly IWebHostEnvironment webHostEnv;

        public ProductController(EshopDbContext eshopDB, IWebHostEnvironment webHostEnvironment)
        {
            eshopDbContext = eshopDB;
            webHostEnv = webHostEnvironment;
        }

        public IActionResult Select()
        {
            IList<Product> products = eshopDbContext.Products.ToList();

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (product != null && product.Image != null)
            {
                FileUpload fu = new FileUpload(webHostEnv.WebRootPath, "img/Products", "image");
                if (fu.CheckFileContent(product.Image) && fu.CheckFileLength(product.Image))
                {
                    product.ImageSource = await fu.FileUploadAsync(product.Image);

                    if (String.IsNullOrWhiteSpace(product.ImageSource) == false)
                    {
                        eshopDbContext.Products.Add(product);

                        await eshopDbContext.SaveChangesAsync();

                        return RedirectToAction(nameof(Select));
                    }
                }
            }

            return View(product);
        }

        public IActionResult Edit(int ID)
        {
            Product prodFromDatabase = eshopDbContext.Products.FirstOrDefault(prod => prod.ID == ID);

            if (prodFromDatabase != null)
            {
                return View(prodFromDatabase);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            Product prodFromDatabase = eshopDbContext.Products.FirstOrDefault(prod => prod.ID == product.ID);

            if (prodFromDatabase != null)
            {
                if (product.Image != null)
                {
                    FileUpload fu = new FileUpload(webHostEnv.WebRootPath, "img/Products", "image");
                    if (fu.CheckFileContent(product.Image) && fu.CheckFileLength(product.Image))
                    {
                        product.ImageSource = await fu.FileUploadAsync(product.Image);
                        if (String.IsNullOrWhiteSpace(product.ImageSource) == false)
                        {
                            prodFromDatabase.ImageSource = product.ImageSource;
                        }
                    }
                }

                prodFromDatabase.Name = product.Name;
                prodFromDatabase.Description = product.Description;
                prodFromDatabase.Price = product.Price;
                prodFromDatabase.Quantity = product.Quantity;

                await eshopDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Select));
            }
            else
            {
                return NotFound();
            }

        }

        public async Task<IActionResult> Delete(int ID)
        {
            Product product = eshopDbContext.Products.Where(prod => prod.ID == ID)
                                                     .FirstOrDefault();

            if (product != null)
            {
                eshopDbContext.Products.Remove(product);

                await eshopDbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Select));
        }
    }
}


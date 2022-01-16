using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UTB.Eshop.Web.Models.Entity;
using UTB.Eshop.Web.Models.Identity;

namespace UTB.Eshop.Web.Models.Database
{
    public class DatabaseInit
    {

        public void Initialization(EshopDbContext eshopDbContext)
        {
            eshopDbContext.Database.EnsureCreated();

            if (eshopDbContext.CarouselItems.Count() == 0)
            {
                IList<CarouselItem> cItems = GenerateCarouselItems();
                foreach (var ci in cItems)
                {
                    eshopDbContext.CarouselItems.Add(ci);
                }
                eshopDbContext.SaveChanges();
            }

            if (eshopDbContext.Products.Count() == 0)
            {
                IList<Product> products = GenerateProducts();
                foreach (var prod in products)
                {
                    eshopDbContext.Products.Add(prod);
                }
                eshopDbContext.SaveChanges();
            }

        }

        public List<CarouselItem> GenerateCarouselItems()
        {
            List<CarouselItem> carouselItems = new List<CarouselItem>();

            CarouselItem ci1 = new CarouselItem()
            {
                //ID = 0,
                ImageSource = "/img/What_is_Information_Technology.jpg",
                ImageAlt = "First slide"
            };
            CarouselItem ci2 = new CarouselItem()
            {
                //ID = 1,
                ImageSource = "/img/Information-Technology-1.jpg",
                ImageAlt = "Second slide"
            };
            CarouselItem ci3 = new CarouselItem()
            {
                //ID = 2,
                ImageSource = "/img/information-technology.jpg",
                ImageAlt = "Third slide"
            };

            carouselItems.Add(ci1);
            carouselItems.Add(ci2);
            carouselItems.Add(ci3);

            return carouselItems;
        }

        public List<Product> GenerateProducts()
        {
            List<Product> products = new List<Product>();

            Product p1 = new Product()
            {
                //ID = 0,
                ImageSource = "/img/Products/Chleb_100_zito_2.jpg",
                Name = "Chleba (žitný)",
                Description = "Žitný chleba ... mňam! Kupujte nebo budete biti, jak žito!",
                Price = 25,
                Quantity = 50,
                Category  = "Pecivo",
                Bought = 1
            };
            Product p2 = new Product()
            {
                //ID = 1,
                ImageSource = "/img/Products/thumb_260x340__masla-a-tuky.jpg",
                Name = "Máslo",
                Description = "Máslo na ten chleba vedle ... ano, na ten žitný ;-)",
                Price = 45,
                Quantity = 10,
                Category = "Pecivo",
                Bought = 2
            };
            Product p3 = new Product()
            {
                //ID = 2,
                ImageSource = "/img/Products/tuna_salad_FJK_bean-mix_prava VS.jpg",
                Name = "Tuna Salad",
                Description = "Mix masa ... mmmmmmmmm!",
                Price = 50,
                Quantity = 25,
                Category = "Maso",
                Bought = 1
            };
            Product p4 = new Product()
            {
                //ID = 3,
                ImageSource = "/img/Products/1615971-3539-1318564-produkty-teaser-nove2.webp",
                Name = "Helmans",
                Description = "Helmans original je lepší než čínský Helmans fake!",
                Price = 100,
                Quantity = 5,
                Category = "Omacky",
                Bought = 1
            };
            Product p5 = new Product()
            {
                //ID = 4,
                ImageSource = "/img/Products/produkty-home.png",
                Name = "Skleničky (200 ml)",
                Description = "Skleničky na to pití, které neprodáváme.",
                Price = 400,
                Quantity = 125,
                Category = "Prislusenstvi",
                Bought = 1
            };

            products.Add(p1);
            products.Add(p2);
            products.Add(p3);
            products.Add(p4);
            products.Add(p5);

            return products;
        }

        public async Task EnsureRoleCreated(RoleManager<Role> roleManager)
        {
            string[] roles = Enum.GetNames(typeof(Roles));

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new Role(role));
            }
        }

        public async Task EnsureAdminCreated(UserManager<User> userManager)
        {
            User user = new User
            {
                UserName = "admin",
                Email = "admin@admin.cz",
                EmailConfirmed = true,
                FirstName = "jmeno",
                LastName = "prijmeni"
            };
            string password = "abc";

            User adminInDatabase = await userManager.FindByNameAsync(user.UserName);

            if (adminInDatabase == null)
            {

                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result == IdentityResult.Success)
                {
                    string[] roles = Enum.GetNames(typeof(Roles));
                    foreach (var role in roles)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
                else if (result != null && result.Errors != null && result.Errors.Count() > 0)
                {
                    foreach (var error in result.Errors)
                    {
                        Debug.WriteLine($"Error during Role creation for Admin: {error.Code}, {error.Description}");
                    }
                }
            }

        }

        public async Task EnsureManagerCreated(UserManager<User> userManager)
        {
            User user = new User
            {
                UserName = "manager",
                Email = "manager@manager.cz",
                EmailConfirmed = true,
                FirstName = "jmeno :-)",
                LastName = "prijmeni :-)"
            };
            string password = "abc";

            User managerInDatabase = await userManager.FindByNameAsync(user.UserName);

            if (managerInDatabase == null)
            {

                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result == IdentityResult.Success)
                {
                    string[] roles = Enum.GetNames(typeof(Roles));
                    foreach (var role in roles)
                    {
                        if (role != Roles.Admin.ToString())
                            await userManager.AddToRoleAsync(user, role);
                    }
                }
                else if (result != null && result.Errors != null && result.Errors.Count() > 0)
                {
                    foreach (var error in result.Errors)
                    {
                        Debug.WriteLine($"Error during Role creation for Manager: {error.Code}, {error.Description}");
                    }
                }
            }

        }

    }
}



using Microsoft.AspNetCore.Identity;
using OnlineStore.Data;
using OnlineStore.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<OnlineStoreUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            OnlineStoreUser user = await UserManager.FindByEmailAsync("admin@onlinestore.com");
            if (user == null)
            {
                var User = new OnlineStoreUser();
                User.Email = "admin@onlinestore.com";
                User.UserName = "admin@onlinestore.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin 
                if (chkUser.Succeeded) { var result = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
            //Add Registered Demo User
            var roleCheck2 = await RoleManager.RoleExistsAsync("User");
            IdentityResult roleResult2;
            if (!roleCheck2) { roleResult2 = await RoleManager.CreateAsync(new IdentityRole("User")); }
            var user2 = await UserManager.FindByEmailAsync("demo@onlinestore.com");
            if (user2 == null)
            {
                var User2 = new OnlineStoreUser();
                User2.Email = "demo@onlinestore.com";
                User2.UserName = "demo@onlinestore.com";
                string user2PWD = "Duser123";
                IdentityResult chkUser2 = await UserManager.CreateAsync(User2, user2PWD);
                //Add default User to Role Admin 
                if (chkUser2.Succeeded) { var result2 = await UserManager.AddToRoleAsync(User2, "Admin"); }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new OnlineStoreContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<OnlineStoreContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                if (context.Category.Any() || context.Manufacturer.Any() || context.Product.Any() || context.ProductCategory.Any() || context.Review.Any() || context.Seller.Any() || context.SellerProduct.Any() || context.UserProduct.Any())
                {
                    return;   // DB has been seeded
                }

                context.Category.AddRange(
                    new Category {/*Id = 1 */ Name = "Домакинство" },
                    new Category {/*Id = 2 */ Name = "Храна" },
                    new Category {/*Id = 3 */ Name = "Пијалоци" },
                    new Category {/*Id = 4 */ Name = "Аптека" },
                    new Category {/*Id = 5 */ Name = "Убавина и Здравје" },
                    new Category {/*Id = 6 */ Name = "Дом и Градина" },
                    new Category {/*Id = 7 */ Name = "Електроника" }
                    );
                context.SaveChanges();

                context.Manufacturer.AddRange(
                    new Manufacturer { Address = "Мајаковски 2/4, Skopje", Name= "Silbo", Country = "Republic of Macedonia", PhotoURL = "/", FoundingDate = DateTime.Parse("11/12/2016"), Email = "/" },
                    new Manufacturer { Address = "Ул. Козле 188, Скопје", Name = "Реплек", Country = "Republic of Macedonia", PhotoURL = "/", FoundingDate = DateTime.Parse("11/2/1945"), Email = "info@replek.mk" },
                    new Manufacturer { Address = "Ул. 808 бр.12 1000 Скопје", Name = "Пивара Скопје", Country = "Republic of Macedonia", PhotoURL = "/", FoundingDate = DateTime.Parse("2-11-1922"), Email = "contact.pivara.skopje@cchellenic.com" }
                    );
                context.SaveChanges();

                context.Seller.AddRange(
                    new Seller { Address = "Кочо Рацин бр.1, 1000 Скопје", Name = "Веропулос Дооел - Скопје", Country = "Republic of Macedonia", PhotoURL = "/", FoundingDate = DateTime.Parse("12/11/1997"), Email = "info@vero.com.mk" },
                    new Seller { Address = "Васил Ѓоргов, бр.16 влез 1 мансарда 1 (ТЦ Зебра) , 1000 Скопје", Name = "Тинекс", Country = "Republic of Macedonia", PhotoURL = "/", FoundingDate = DateTime.Parse("12/1/2002"), Email = "tinex@tinex.com.mk" },
                    new Seller { Address = "Васил Москов бр. 314, 1000 Скопје", Name = "Пиљара Пало", Country = "Republic of Macedonia", PhotoURL = "/", FoundingDate = DateTime.Parse("23.12.2023"), Email = "/" }
                    );
                context.SaveChanges();

                context.Product.AddRange(
                    new Product { Description = "Жолт компир 1 кг", ManufacturerId = 3, Name = "Жолт компир", PhotoURL = "/", UserManualURL = "/" },
                    new Product { Description = "Силбо бел леб 500g.", ManufacturerId = context.Manufacturer.Single(d => d.Name == "Silbo").Id, Name = "Силбо бел леб", PhotoURL = "/", UserManualURL = "/" },
                    new Product { Description = "Силбо арабо леб, 400гр", ManufacturerId = context.Manufacturer.Single(d => d.Name == "Silbo").Id, Name = "Силбо арабо леб", PhotoURL = "/", UserManualURL = "/" }
                    );
                context.SaveChanges();

                context.ProductCategory.AddRange(
                    new ProductCategory {ProductId = context.Product.Single(d => d.Name == "Жолт компир").Id, CategoryId = context.Category.Single(d => d.Name == "Храна").Id },
                    new ProductCategory {ProductId = context.Product.Single(d => d.Name == "Силбо бел леб").Id, CategoryId = context.Category.Single(d => d.Name == "Храна").Id }
                    );
                context.SaveChanges();

                context.SellerProduct.AddRange(
                    new SellerProduct { ProductId = context.Product.Single(d => d.Name == "Силбо бел леб").Id, SellerId = context.Seller.Single(d => d.Name == "Пиљара Пало").Id, Price = 45, PublishDate = DateTime.Now },
                    new SellerProduct { ProductId = context.Product.Single(d => d.Name == "Силбо бел леб").Id, SellerId = context.Seller.Single(d => d.Name == "Тинекс").Id, Price = 57, PublishDate = DateTime.Now },
                    new SellerProduct { ProductId = context.Product.Single(d => d.Name == "Силбо бел леб").Id, SellerId = context.Seller.Single(d => d.Name == "Веропулос Дооел - Скопје").Id, Price = 70, PublishDate = DateTime.Now }
                    );
                context.SaveChanges();
                context.Review.AddRange(
                    new Review { ProductId = context.Product.Single(d => d.Name == "Жолт компир").Id, Rating = 2, UserId = "/", Comment = "сафгд "}
                    );
                context.SaveChanges();
            }
        }
    }
}

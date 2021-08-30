using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCore.Data.Entities;
using WebCore.Data.Enums;

namespace WebCore.Data.Extensions
{
    public static class ModelBuilderEntensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //Apconfig
            modelBuilder.Entity<AppConfig>().HasData(
                new AppConfig() { Key = "HomeTitle", Value = "This is home page of MyWeb " },
                new AppConfig() { Key = "HomeKeyword", Value = "This is keyword of MyWeb " },
                new AppConfig() { Key = "HomeDescription", Value = "This is home page of MyWeb " }
            );
            //Language
            modelBuilder.Entity<Language>().HasData(
               new Language() { Id = "vi-VN", Name = "Tiếng Việt", IsDefault = true },
               new Language() { Id = "en-US", Name = "English", IsDefault = false }
           );



            // Category
            modelBuilder.Entity<Category>().HasData(
                 new Category()
                 {
                     Id = 1,
                     IsShowonHome = true,
                     ParentId = null,
                     SortOrder = 1,
                     Status = Status.Active,
                 },
                 new Category()
                 {
                     Id = 2,
                     IsShowonHome = true,
                     ParentId = null,
                     SortOrder = 2,
                     Status = Status.Active,
                 }
            );

            modelBuilder.Entity<CategoryTranslation>().HasData(
                new CategoryTranslation() {Id=1 ,CategoryId=1, Name = "Áo Nam", LanguageId = "vi-VN", SeoAlias = "ao-nam", SeoDescription = "Sản phẩm áo thời trang nam", SeoTitle = "Sản phẩm áo thời trang nam" },
                new CategoryTranslation() { Id = 2, CategoryId = 1, Name = "Men T-Shirt", LanguageId = "en-US", SeoAlias = "men-shirt", SeoDescription = "The shirt products for men", SeoTitle = "The shirt products for men" },
                new CategoryTranslation() { Id = 3, CategoryId = 2, Name = "Áo nữ", LanguageId = "vi-VN", SeoAlias = "ao-nu", SeoDescription = "Sản phẩm áo thời trang nữ", SeoTitle = "Sản phẩm áo thời trang nữ" },
                new CategoryTranslation() { Id = 4, CategoryId = 2, Name = "Women T-Shirt", LanguageId = "en-US", SeoAlias = "women-shirt", SeoDescription = "The shirt products for women", SeoTitle = "The shirt products for women" }
            );




            // Product
            modelBuilder.Entity<Product>().HasData(
                 new Product()
                 {
                     Id = 1,
                     DateCreated = DateTime.Now,
                     OriginalPrice = 100000,
                     Price = 200000,
                     Stock = 0,
                     ViewCount = 0,
                 }
            );

            modelBuilder.Entity<ProductInCategory>().HasData(
                new ProductInCategory() {ProductId=1, CategoryId = 1 }
            );
            modelBuilder.Entity<ProductTranslation>().HasData(
                new ProductTranslation()
                {
                    Id=1,
                    ProductId = 1,
                    Name = "Áo sơ mi nam trắng Việt Tiến",
                    LanguageId = "vi-VN",
                    SeoAlias = "ao-so-mi-nam-trang-viet-tien",
                    SeoDescription = "Áo sơ mi nam trắng Việt Tiến",
                    SeoTitle = "Áo sơ mi nam trắng Việt Tiến",
                    Details = "Áo sơ mi nam trắng Việt Tiến",
                    Description = "Áo sơ mi nam trắng Việt Tiến"
                },

                new ProductTranslation()
                {
                    Id = 2,
                    ProductId = 1,
                    Name = "Viet Tien Men T-Shirt",
                    LanguageId = "en-US",
                    SeoAlias = "viet-tien-men-t-shirt",
                    SeoDescription = "Viet Tien Men T-Shirt",
                    SeoTitle = "Viet Tien Men T-Shirt",
                    Details = "Viet Tien Men T-Shirt",
                    Description = "Viet Tien Men T-Shirt"
                }
            );

            // User

            // any guid
            var ADMIN_ID = new Guid("B38060F2-8B1C-47AE-80AA-2CF1B518B812") ;
            // any guid, but nothing is against to use the same one
            var ROLE_ID = new Guid("0D5B7850-46C1-4C80-99C4-D94FC38A3EA7");
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = ROLE_ID,
                Name = "admin",
                NormalizedName = "admin",
                Description="Adminstrator Role ",
            });

            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "dinhson14399@gmail.com",
                NormalizedEmail = "dinhson14399@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "123"),
                SecurityStamp = string.Empty,
                FirstName="Dinh",
                LastName="Son",
                Dob=new DateTime(1999,03,14)
            });;

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });
        }
    }
}

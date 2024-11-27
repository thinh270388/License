using License.APIs.Helpers;
using License.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace License.APIs.Datas
{
    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();
        public async Task SeedAsync(ApplicationDbContext context, ILogger<ApplicationDbContextSeed> logger, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            if (!context.Roles.Any())
            {
                await roleManager.CreateAsync(new ApplicationRole() { Name = UserRoles.ADMIN });
                await roleManager.CreateAsync(new ApplicationRole() { Name = UserRoles.USER });
            }

            if (!context.Users.Any())
            {
                string email = configuration["DefaultAccount:Email"]!;
                var user = new ApplicationUser()
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = configuration["DefaultAccount:PhoneNumber"]!,
                    FirstName = configuration["DefaultAccount:FirstName"]!,
                    LastName = configuration["DefaultAccount:LastName"]!
                };
                await userManager.CreateAsync(user, configuration["DefaultAccount:Password"]!);
                await userManager.AddToRoleAsync(user, UserRoles.ADMIN);
            }

            //if (!context.Categories.Any())
            //{
            //    var category = new Category()
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = $"Danh mục 1",
            //        Description = string.Empty,
            //        CreatedDate = DateTime.Now,
            //        UpdatedDate = DateTime.Now
            //    };
            //    context.Categories.Add(category);

            //    for (int i = 2; i <= 20; i++)
            //    {
            //        var categoryAdd = new Category()
            //        {
            //            Id = Guid.NewGuid(),
            //            Name = $"Danh mục {i}",
            //            Description = string.Empty,
            //            CreatedDate = DateTime.Now,
            //            UpdatedDate = DateTime.Now
            //        };
            //        context.Categories.Add(categoryAdd);
            //    }

            //    var unit = new Unit()
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Kg",
            //        Description = "Ki-lô-gam",
            //        CreatedDate = DateTime.Now,
            //        UpdatedDate = DateTime.Now
            //    };
            //    context.Units.Add(unit);

            //    var supplier = new Supplier()
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Công ty A",
            //        Address = string.Empty,
            //        BankAccount = string.Empty,
            //        Description = string.Empty,
            //        Email = string.Empty,
            //        Fax = string.Empty,
            //        Phone = string.Empty,
            //        Tax = string.Empty,
            //        Website = string.Empty,
            //        CreatedDate = DateTime.Now,
            //        UpdatedDate = DateTime.Now
            //    };
            //    context.Suppliers.Add(supplier);

            //    context.Products.Add(new Product()
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Sản phẩm 1",
            //        UnitId = unit.Id,
            //        CategoryId = category.Id,
            //        SupplierId = supplier.Id,
            //        InputPrice = 55000,
            //        OutputPrice = 78000,
            //        Description = string.Empty,
            //        Category = category,
            //        Inventory = 0,
            //        Status = Sales.Models.Enums.StatusProduct.Online,
            //        Supplier = supplier,
            //        CreatedDate = DateTime.Now,
            //        UpdatedDate = DateTime.Now
            //    });
            //}
            await context.SaveChangesAsync();
        }
    }
}


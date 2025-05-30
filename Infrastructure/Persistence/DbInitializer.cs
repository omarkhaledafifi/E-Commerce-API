using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Persistence.Identity;
using System.Text.Json;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreContext _storeContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly StoreIdentityContext _storeIdentityContext;
        public DbInitializer (StoreContext storeContext,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            StoreIdentityContext storeIdentityContext)
        {
            _storeContext = storeContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _storeIdentityContext = storeIdentityContext;
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Create DataBase IF It doesn't Exist & Applying Any Pending Migrations
                if (_storeContext.Database.GetPendingMigrations().Any())
                    await _storeContext.Database.MigrateAsync();

                // Apply Data Seeding 
                if (!_storeContext.ProductTypes.Any())
                {
                    // Read Types From File  as string 
                    var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");


                    // Transform into C# Objects
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);


                    //Add to DB & save Changes 
                    if (types is not null && types.Any())
                    {
                        await _storeContext.ProductTypes.AddRangeAsync(types);
                        await _storeContext.SaveChangesAsync();
                    }
                }

                if (!_storeContext.ProductBrands.Any())
                {
                    // Read Types From File  as string 
                    var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");


                    // Transform into C# Objects
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);


                    //Add to DB & save Changes 
                    if (brands is not null && brands.Any())
                    {
                        await _storeContext.ProductBrands.AddRangeAsync(brands);
                        await _storeContext.SaveChangesAsync();
                    }
                }

                if (!_storeContext.Products.Any())
                {
                    // Read Types From File  as string 
                    var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");


                    // Transform into C# Objects
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);


                    //Add to DB & save Changes 
                    if (products is not null && products.Any())
                    {
                        await _storeContext.Products.AddRangeAsync(products);
                        await _storeContext.SaveChangesAsync();
                    }
                }

                if (!_storeContext.DeliveryMethods.Any())
                {
                    // Read Types From File  as string 
                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\delivery.json");


                    // Transform into C# Objects
                    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(data);


                    //Add to DB & save Changes 
                    if (methods is not null && methods.Any())
                    {
                        await _storeContext.DeliveryMethods.AddRangeAsync(methods);
                        await _storeContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task InitializeIdentityAsync()
        {
            // Seed Default User & Roles 

            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            if (!_userManager.Users.Any())
            {
                var superAdminUser = new User
                {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123465789"
                };

                var adminUser = new User
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "0123465789"
                };

                await _userManager.CreateAsync(superAdminUser, "Passw0rd");
                await _userManager.CreateAsync(adminUser, "Passw0rd");


                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(adminUser, "Admin");


            }
        }
    }
}

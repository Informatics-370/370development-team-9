using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
namespace TrackwiseAPI
{

    public class Program
    {
        // Other code...

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<TwDbContext>();
    var customers = scope.ServiceProvider.GetRequiredService<Customer>;

    string email = "customer@gmail.com";
    string password = "Test1234";
    var customerId = Guid.NewGuid().ToString();

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new AppUser();
        user.Id = customerId;
        user.UserName = email;
        user.Email = email;

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Customer");

        var customer = new Customer
        {
            Customer_ID = customerId,
            Name = "Default Customer",
            LastName = "Default",
            Email = email,
            Password = password
        };

        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();
    }
}

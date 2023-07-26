using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TrackwiseAPI.Models.Factory;
using TrackwiseAPI.Models.BingMapsAPI;
using Microsoft.Extensions.Configuration;
using TrackwiseAPI.Models.Password;
using TrackwiseAPI.Models.Email;

public class Startup
{ 

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IMailService, MailService>();
        services.AddTransient<IEmailService, EmailService>();
        //services.AddTransient<IEmailService, MockEmailService>();
        services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
        services.Configure<MailSettings>(Configuration.GetSection(nameof(MailSettings)));
        services.AddHttpClient<TruckRouteService>();
        services.AddSingleton<TruckRouteService>();
        services.AddCors(options => options.AddDefaultPolicy(
            include =>
            {
                include.AllowAnyHeader();
                include.AllowAnyMethod();
                include.AllowAnyOrigin();
            }));

        services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        // Read the Bing Maps API key from appsettings.json
        string bingMapsApiKey = Configuration.GetSection("BingMapsSettings:ApiKey").Value;

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Add Bearer Token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{ }
                }
            });
        });

        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = true;
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<TwDbContext>()
        .AddDefaultTokenProviders();

        services.AddAuthentication()
            .AddCookie()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                };
            });

        services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, AppUserClaimsPrincipalFactory>();

        services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(3));

        services.AddDbContext<TwDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ITruckRepository, TruckRepository>();
        services.AddScoped<ITrailerRepository, TrailerRepository>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        // Using Task.Run to perform the asynchronous operations and Wait to block the Configure method.
        Task.Run(async () =>
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var Roles = new[] { "Admin", "Client", "Customer", "Driver" };

                foreach (var role in Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                string email = "admin@admin.com";
                string password = "Test1234";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new AppUser();
                    user.UserName = email;
                    user.Email = email;

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }).Wait();
    }

}


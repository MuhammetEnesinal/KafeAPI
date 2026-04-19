using AspNetCoreRateLimit;
using FluentValidation;
using KafeAPI.Application.Dtos.CafeInfoDtos;
using KafeAPI.Application.Dtos.CategoryDtos;
using KafeAPI.Application.Dtos.MenuItemDtos;
using KafeAPI.Application.Dtos.OrderDtos;
using KafeAPI.Application.Dtos.OrderItemDtos;
using KafeAPI.Application.Dtos.TableDtos;
using KafeAPI.Application.Helpers;
using KafeAPI.Application.Interfaces;
using KafeAPI.Application.Mapping;
using KafeAPI.Application.Services.Abstract;
using KafeAPI.Application.Services.Concrete;
using KafeAPI.Persistence.Context;
using KafeAPI.Persistence.Context.Identity;
using KafeAPI.Persistence.Middlewares;
using KafeAPI.Persistence.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Text;

var builder = WebApplication.CreateBuilder(args);







builder.Services.AddDbContext<AppDbContext>(opt =>
{
    
    var conf = builder.Configuration.GetConnectionString("DefaultConnection");
    opt.UseSqlServer(conf);
});



builder.Services.AddDbContext<AppIdentityDbContext>(opt =>
{

    var conf = builder.Configuration.GetConnectionString("DefaultConnection");
    opt.UseSqlServer(conf);
});

builder.Services.AddIdentity<AppIdentityUser, AppIdentityRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 6;
}).AddEntityFrameworkStores< AppIdentityDbContext>().AddDefaultTokenProviders();


builder.Services.AddControllers();



builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<TokenHelpers>();


builder.Services.AddScoped<IMenuItemServices, MenuItemServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<ITableServices, TableServices>();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<IOrderItemServices, OrderItemServices>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<ICafeInfoServices, CafeInfoServices>();








builder.Services.AddAutoMapper(typeof(GeneralMapping));


builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryDto>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryDto>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateMenuItemDto>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateMenuItemDto>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateTableDto>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateTableDto>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderDto>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateOrderDto>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderItemDto>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateOrderItemDto>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateCafeInfoDto>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCafeInfoDto>();


builder.Services.AddOpenApi();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();


//Serilog config
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();




builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);
builder.Host.UseSerilog();
builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(
builder.Configuration.GetSection("IpRateLimiting"));

builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<AppIdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<AppIdentityUser>>();

        string roleName = "admin";

        // 1. Rolü Kontrol Et ve Yoksa Oluþtur
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var roleResult = await roleManager.CreateAsync(new AppIdentityRole { Name = roleName });
            if (!roleResult.Succeeded)
            {
                Console.WriteLine("!!! DÝKKAT: Rol oluþturulamadý !!!");
                foreach (var err in roleResult.Errors) Console.WriteLine($"- {err.Description}");
            }
        }

        // 2. Admin Kullanýcýsýný Kontrol Et ve Yoksa Oluþtur
        string adminEmail = "admin@kafeapi.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new AppIdentityUser
            {
                UserName = "admin",
                Email = adminEmail,
                Name = "System",     
                Surname = "Admin"     
            };

            var createResult = await userManager.CreateAsync(adminUser, "admin123");

            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, roleName);
                Console.WriteLine(">>> BAÞARILI: Admin kullanýcýsý ve rolü eklendi! <<<");
            }
            else
            {
                Console.WriteLine("!!! DÝKKAT: Kullanýcý oluþturulamadý. Sebepleri: !!!");
                foreach (var error in createResult.Errors)
                {
                    Console.WriteLine($"HATA DETAYI: {error.Code} - {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine(">>> BÝLGÝ: Admin kullanýcýsý zaten veritabanýnda mevcut. <<<");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"!!! KRÝTÝK HATA !!! Seed Data çalýþýrken uygulama patladý: {ex.Message}");
    }
}



app.MapScalarApiReference(opt =>
{
    opt.Title = "Kafe API v1";
    opt.Theme = ScalarTheme.BluePlanet;
    opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
});


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseIpRateLimiting();
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<SerilogMiddleware>();
app.MapControllers();

app.Run();

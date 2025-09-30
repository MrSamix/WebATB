using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebATB.Data;
using WebATB.Data.Entities.Identity;
using WebATB.Extensions;
using WebATB.Interfaces;
using WebATB.Services;

var builder = WebApplication.CreateBuilder(args);

// Google Auth
builder.Services.AddAuthentication()
    .AddCookie()
    .AddGoogle(opt =>
    {
        opt.ClientId = builder.Configuration["GoogleKeys:ClientId"];
        opt.ClientSecret = builder.Configuration["GoogleKeys:ClientSecret"];
        opt.SignInScheme = IdentityConstants.ExternalScheme; // explicit
        opt.SaveTokens = true;
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppATBDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlDb")));
//builder.Services.AddDbContext<AppATBDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionATB")));

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AppATBDbContext>()
    .AddDefaultTokenProviders();

// Примусово перевіряємо cookie при кожному запиті, щоб зміни застосовувалися негайно
builder.Services.Configure<SecurityStampValidatorOptions>(o =>
{
    o.ValidationInterval = TimeSpan.Zero;
});

// Адмінка - тільки для адміністраторів
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapAreaControllerRoute(
    name: "MyAreaAdmin",
    areaName: "Admin",
    pattern: "Admin/{controller=Users}/{action=Index}/{id?}")
    .RequireAuthorization("AdminOnly");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}")
    .WithStaticAssets();

Dictionary<string, string> imageSizes = new()
{
    { "ImagesDir", "images" },
    { "AvatarDir", "avatars" },
};

foreach (var (key, value) in imageSizes)
{
    var dirName = builder.Configuration.GetValue<string>(key) ?? value;

    var dir = Path.Combine(Directory.GetCurrentDirectory(), dirName);
    Directory.CreateDirectory(dir);

    // Дозволяємо доступ до файлів в папці images по шляху /images
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(dir),
        RequestPath = $"/{value}"
    });
}

await app.SeedDataAsync();

app.Run();

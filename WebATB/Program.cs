using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebATB.Data;
using WebATB.Data.Entities.Identity;
using WebATB.Extensions;
using WebATB.Interfaces;
using WebATB.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<AppATBDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("MyConnectionATB")));
builder.Services.AddDbContext<AppATBDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionATB")));

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

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}")
    .WithStaticAssets();

// images
var imagesDirName = builder.Configuration.GetValue<string>("ImagesDir") ?? "images";
var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), imagesDirName);
Directory.CreateDirectory(imagesDir);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(imagesDir),
    RequestPath = "/images"
});

// avatars
var avatarDirName = builder.Configuration.GetValue<string>("AvatarDir") ?? "avatars";
var avatarDir = Path.Combine(Directory.GetCurrentDirectory(), avatarDirName);
Directory.CreateDirectory(avatarDir);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(avatarDir),
    RequestPath = "/avatars"
});

await app.SeedDataAsync();

app.Run();

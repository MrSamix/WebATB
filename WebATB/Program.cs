using Microsoft.EntityFrameworkCore;
using WebATB.Data;
using WebATB.Interfaces;
using WebATB.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<AppATBDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("MyConnectionATB")));
builder.Services.AddDbContext<AppATBDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionATB")));

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

var dirName = builder.Configuration.GetValue<string>("ImagesDir") ?? "images";

var dir = Path.Combine(Directory.GetCurrentDirectory(), dirName);
Directory.CreateDirectory(dir);
// Дозволяємо доступ до файлів в папці images по шляху /images
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(dir),
    RequestPath = "/images"
});


app.Run();

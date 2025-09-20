using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using WebATB.Interfaces;

namespace WebATB.Services;

public class ImageService(IConfiguration configuration) : IImageService
{
    public async Task DeleteImageAsync(string name)
    {
        var sizes = configuration.GetRequiredSection("ImageSizes").Get<List<int>>();
        var dir = Path.Combine(Directory.GetCurrentDirectory(), configuration["ImagesDir"]!);

        if (sizes == null || sizes.Count == 0)
        {
            sizes = new List<int> { 256, 512, 1024 };
        }

        Task[] tasks = sizes
            .AsParallel()
            .Select(size =>
            {
                return Task.Run(() =>
                {
                    var path = Path.Combine(dir, $"{size}_{name}");
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                });
            })
            .ToArray();

        await Task.WhenAll(tasks);
    }

    public async Task<string> SaveImageAsync(IFormFile file)
    {
        using MemoryStream ms = new();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();

        var imageName = await SaveImageAsync(bytes);
        return imageName;
    }

    public async Task<string> SaveImageAsync(string base64)
    {
        if (base64.Contains(','))
        {
            base64 = base64[(base64.IndexOf(',') + 1)..];
        }
        var bytes = Convert.FromBase64String(base64);
        var imageName = await SaveImageAsync(bytes);
        return imageName;
    }

    public async Task<string> SaveAvatarAsync(IFormFile file)
    {
        using MemoryStream ms = new();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();

        string imageName = Guid.NewGuid().ToString() + ".webp";
        var sizes = configuration.GetRequiredSection("ImageSizes").Get<List<int>>();
        if (sizes == null || sizes.Count == 0)
        {
            sizes = new List<int> { 256, 512, 1024 };
        }

        var avatarDir = configuration.GetValue<string>("AvatarDir") ?? "avatars";

        Task[] tasks = sizes
            .AsParallel()
            .Select(s => SaveImageToDirAsync(bytes, imageName, s, avatarDir))
            .ToArray();

        await Task.WhenAll(tasks);
        return imageName;
    }

    private async Task<string> SaveImageAsync(byte[] bytes)
    {
        string imageName = Guid.NewGuid().ToString() + ".webp";
        var sizes = configuration.GetRequiredSection("ImageSizes").Get<List<int>>();
        if (sizes == null || sizes.Count == 0)
        {
            sizes = new List<int> { 256, 512, 1024 };
        }
        Task[] tasks = sizes
            .AsParallel()
            .Select(s => SaveImageAsync(bytes, imageName, s))
            .ToArray();
        await Task.WhenAll(tasks);
        return imageName;

    }
    
    /// <summary>
    /// Зберігає зображення у різних розмірах
    /// </summary>
    /// <param name="bytes">набір байтів зображення</param>
    /// <param name="name">назва фото</param>
    /// <param name="size">розмір із яким зберігати</param>
    /// <returns></returns>
    private async Task SaveImageAsync(Byte[] bytes, string name, int size)
    {
        var dirName = configuration.GetValue<string>("ImagesDir") ?? "images";
        var baseDir = Path.Combine(Directory.GetCurrentDirectory(), dirName);
        Directory.CreateDirectory(baseDir);
        var path = Path.Combine(baseDir, $"{size}_{name}");
        using var image = Image.Load(bytes);
        image.Mutate(imgContext => {
            imgContext.Resize(new ResizeOptions
            {
                Size = new Size(size, size),
                Mode = ResizeMode.Max
            });
        });
        await image.SaveAsWebpAsync(path);
    }

    private async Task SaveImageToDirAsync(Byte[] bytes, string name, int size, string dirName)
    {
        var baseDir = Path.Combine(Directory.GetCurrentDirectory(), dirName);
        Directory.CreateDirectory(baseDir);
        var path = Path.Combine(baseDir, $"{size}_{name}");
        using var image = Image.Load(bytes);
        image.Mutate(imgContext => {
            imgContext.Resize(new ResizeOptions
            {
                Size = new Size(size, size),
                Mode = ResizeMode.Max
            });
        });
        await image.SaveAsWebpAsync(path);
    }
}

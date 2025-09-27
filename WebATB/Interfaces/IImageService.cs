namespace WebATB.Interfaces;

public interface IImageService
{
    Task<string> SaveImageAsync(IFormFile file, string options="ImagesDir");
    Task<string> SaveImageAsync(string base64);

    Task DeleteImageAsync(string name);
}

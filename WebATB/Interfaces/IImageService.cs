namespace WebATB.Interfaces;

public interface IImageService
{
    Task<string> SaveImageAsync(IFormFile file);
    Task<string> SaveImageAsync(string base64);

    Task DeleteImageAsync(string name);
}

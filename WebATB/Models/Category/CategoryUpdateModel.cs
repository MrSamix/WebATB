using System.ComponentModel.DataAnnotations;

namespace WebATB.Models.Category;

public class CategoryUpdateModel
{
    // Id потрібен для того, щоб знати яку категорію оновлюємо
    public int Id { get; set; }
    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Вкажіть назву категорії")]
    public string Name { get; set; } = string.Empty;

    //Фото яке на даний момент є в категорії
    public string? Image { get; set; }

    [Display(Name = "Оберіть файл для фото")]
    public IFormFile? ImageFile { get; set; }
    public string? Description { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace WebATB.Models.Category;

public class CategoryCreateModel
{
    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Вкажіть назву категорії")]
    public string Name { get; set; } = string.Empty;
    [Display(Name = "Оберіть файл для фото")]
    public IFormFile? Image { get; set; }
    [Display(Name = "Введіть опис для категорії")]
    public string? Description { get; set; }
}

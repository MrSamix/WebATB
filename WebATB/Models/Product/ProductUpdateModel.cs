using System.ComponentModel.DataAnnotations;
using WebATB.Models.Helpers;

namespace WebATB.Models.Product
{
    public class ProductUpdateModel
    {
        public int Id { get; set; }
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Вкажіть назву")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Опис")]
        public string? Description { get; set; } = string.Empty;
        [Display(Name = "Ціна")]
        [Required(ErrorMessage = "Вкажіть ціну")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ціна має бути більшою за 0")]
        public decimal Price { get; set; }
        [Display(Name = "Категорія")]
        [Range(1, int.MaxValue, ErrorMessage = "Оберіть категорію")]
        public int CategoryId { get; set; }
        //Щоб бачити список, який ми хочемо показати в дропдауні
        public List<SelectItemHelper> Categories { get; set; } = new List<SelectItemHelper>();

        public string[]? Images { get; set; }
    }
}

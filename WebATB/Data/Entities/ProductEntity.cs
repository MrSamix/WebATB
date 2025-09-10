using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebATB.Data.Entities;

//[Table("tblProducts")]
public class ProductEntity
{
    //[Key]
    public int Id { get; set; }
    [Required, StringLength(255)]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    //[ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }
    public CategoryEntity Category { get; set; }
    public bool IsDeleted { get; set; } = false;

    // conn
    public ICollection<ProductImageEntity>? ProductsImages { get; set; }
}

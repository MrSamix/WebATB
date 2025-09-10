using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebATB.Data.Entities;

//[Table("tblProducts")]
public class ProductImageEntity
{
    //[Key]
    public int Id { get; set; }
    [Required, StringLength(255)]
    public string Path { get; set; }
    public short Priority { get; set; }
    public int ProductId { get; set; }
    // conn
    public ProductEntity Product { get; set; }
}

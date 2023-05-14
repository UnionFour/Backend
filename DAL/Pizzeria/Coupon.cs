using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DAL.Pizzeria;

[Table("coupons")]
public class Coupon
{
    [Key]
    [Column("promocode")]
    public string Promocode { get; set; } =null!;
    
    [Column("validfrom")]
    public DateTime ValidFrom { get; set; }
    
    [Column("validto")]
    public DateTime ValidTo { get; set; }
    
    [Column("maxuses")]
    public int MaxUses { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Products")]
    public virtual Product Product { get; set; } = null!;
}
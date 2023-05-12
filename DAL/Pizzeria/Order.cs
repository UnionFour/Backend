using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DAL.Pizzeria;

[Table("orders")]
public partial class Order
{
    [Key]
    [Column("orderid")]
    public long OrderId { get; set; }

    [Column("cost", TypeName = "money")]
    public decimal Cost { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("createdate")]
    public DateOnly Createdate { get; set; }

    [Column("preparationdate")]
    public DateOnly Preparationdate { get; set; }

    [Column("completingdate")]
    public DateOnly Completingdate { get; set; }

    [Column("promocode")]
    public string? Promocode { get; set; }

    [Column("userid")]
    public long Userid { get; set; }

    [InverseProperty("Orders")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [ForeignKey("Userid")]
    [InverseProperty("Orders")]
    public virtual User User { get; set; } = null!;
}

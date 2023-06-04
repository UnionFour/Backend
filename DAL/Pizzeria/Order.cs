using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.DAL.Pizzeria.Enums;

namespace Backend.DAL.Pizzeria;

[Table("orders")]
public class Order
{
    [Key]
    [Column("orderid")]
    public Guid OrderId { get; set; }

    [Column("cost", TypeName = "money")]
    public decimal Cost { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("createdate")]
    public DateTime Createdate { get; set; }

    [Column("preparationdate")]
    public DateTime Preparationdate { get; set; }

    [Column("completingdate")]
    public DateTime Completingdate { get; set; }

    [Column("promocode")]
    public string? Promocode { get; set; }

    [Column("userid")]
    public Guid Userid { get; set; }
    
    [Column("extradition")]
    public string Extradition { get; set; }

    // [InverseProperty("Orders")]
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public ICollection<OrdersProducts> OrdersProducts { get; set; } = new List<OrdersProducts>();

    [ForeignKey("Userid")]
    [InverseProperty("Orders")]
    public virtual User User { get; set; } = null!;

    // [ForeignKey("Promocode")]
    // [InverseProperty("Coupons")]
    // public virtual Coupon Coupon { get; set; } = null!;
}

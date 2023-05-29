using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.DAL.Pizzeria;

[Table("orders_products")]
public class OrdersProducts
{
	[Key]
	[Column("id")]
	public Guid Id { get; set; }
	
	[Column("amount")]
	public int Amount { get; set; }

	[ForeignKey("Order")]
	[Column("orderid")]
	public Guid OrderId { get; set; }

	[Column("productname")]
	public string? ProductName { get; set; }
	
	[Column("productid")]
	public Guid ProductId { get; set; }

	public virtual Order Order { get; set; } = null!;
	
    public virtual Product Product { get; set; } = null!;
}
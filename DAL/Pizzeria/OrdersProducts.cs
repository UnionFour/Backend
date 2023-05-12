using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.DAL.Pizzeria;

[Table("orders_products")]
public class OrdersProducts
{
	[Key]
	[Column("id")]
	public long Id { get; set; }

	[Column("orderid")]
	public long OrderId { get; set; }

	[Column("productname")]
	public string? ProductName { get; set; }

	[Column("productid")]
	public long ProductId { get; set; }

	public virtual Order Order { get; set; } = null!;
	
    public virtual Product Product { get; set; } = null!;
}
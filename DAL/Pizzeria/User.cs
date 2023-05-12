using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.DAL.Pizzeria;

[Table("users")]
[Index("Email", Name = "users_email_key", IsUnique = true)]
[Index("Phone", Name = "users_phone_key", IsUnique = true)]
public partial class User
{
	[Key]
	[Column("userid")]
	public long Userid { get; set; }

	[Column("name")]
	public string? Name { get; set; }

	[Column("address")]
	public string? Address { get; set; }

	[Column("phone")]
	[StringLength(15)]
	public string Phone { get; set; } = null!;

	[Column("birth")]
	public DateOnly? Birth { get; set; }

	[Column("email")]
	public string? Email { get; set; }

	[Column("gamepoints")]
	public long? Gamepoints { get; set; }

	// [InverseProperty("User")]
	// public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();

	[InverseProperty("User")]
	public ICollection<Order> Orders { get; set; } = new List<Order>();
}
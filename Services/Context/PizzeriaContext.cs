using Backend.DAL.Pizzeria;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Context;

public class PizzeriaContext : DbContext
{
	public PizzeriaContext(DbContextOptions<PizzeriaContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Order>? Orders { get; set; }
	public virtual DbSet<Product>? Products { get; set; }
	public virtual DbSet<User>? Users { get; set; }
	public virtual DbSet<Coupon>? Coupons { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
			.HasPostgresEnum("amount_type", new[] { "kilo", "liter", "piece" })
			.HasPostgresEnum("dough", new[] { "thick", "middle", "big" })
			.HasPostgresEnum("employee", new[] { "manager", "Technologist", "receiver", "Baker", "courier" })
			.HasPostgresEnum("employee_status", new[] { "work", "fired" })
			.HasPostgresEnum("order_extradition", new[] { "delivery", "pickUp" })
			.HasPostgresEnum("status", new[] { "new", "accepted", "in_work", "done", "in_delivery", "paid" })
			.HasPostgresEnum("work_shift_type", new[] { "5/2", "2/2", "piece-rate" });
		

		modelBuilder.Entity<Order>()
			.HasMany(o => o.Products)
			.WithMany(p => p.Orders)
			.UsingEntity<OrdersProducts>(
				r => r.HasOne<Product>().WithMany().HasForeignKey("ProductId", "ProductName"),
				l => l.HasOne<Order>().WithMany().HasForeignKey(x => x.OrderId)
		);

		// modelBuilder.Entity<Coupon>()
		// 	.HasOne(c => c.Product)
		// 	.WithMany(p => p.Coupons);
	}
}
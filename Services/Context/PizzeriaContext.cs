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
	public virtual DbSet<OrdersProducts>? OrdersProducts { get; set; }
	public virtual DbSet<Ingredient>? Ingredients { get; set; }
	public virtual DbSet<IngredientsProducts>? IngredientsProducts { get; set; }

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
		

		// modelBuilder.Entity<Order>()
		// 	.HasMany(o => o.Products)
		// 	.WithMany(p => p.Orders)
		// 	.UsingEntity<OrdersProducts>(
		// 		r => r.HasOne<Product>().WithMany().HasForeignKey("ProductId", "ProductName"),
		// 		l => l.HasOne<Order>().WithMany().HasForeignKey(x => x.OrderId)
		// );

		// modelBuilder.Entity<Coupon>()
		// 	.HasOne(c => c.Product)
		// 	.WithMany(p => p.Coupons);

		
		modelBuilder.Entity<Order>()
			.HasMany(o => o.Products)
			.WithMany(p => p.Orders)
			.UsingEntity<OrdersProducts>(
				i => i
					.HasOne(op => op.Product)
					.WithMany(p => p.OrdersProducts)
					.HasForeignKey("ProductId", "ProductName"),
				i => i
					.HasOne(op => op.Order)
					.WithMany(o => o.OrdersProducts)
					.HasForeignKey(op => op.OrderId),
				i =>
				{
					i.HasKey(op => new { op.ProductName, op.ProductId, op.OrderId });
				});

		modelBuilder.Entity<Product>()
			.HasMany(p => p.Ingredients)
			.WithMany(i => i.Products)
			.UsingEntity<IngredientsProducts>(
				j => j
					.HasOne(ip => ip.Ingredient)
					.WithMany(i => i.IngredientsProducts)
					.HasForeignKey(ip => ip.IngredientID),
				j => j
					.HasOne(ip => ip.Product)
					.WithMany(p => p.IngredientsProducts)
					.HasForeignKey("ProductID", "ProductName"),
				j =>
				{
					j.HasKey(ip => new { ip.IngredientID, ip.ProductID, ip.ProductName });
				});
	}
}
using Backend.DAL.Pizzeria;
using Backend.DAL.Pizzeria.Enums;
using Backend.DTO.Entities;
using Backend.Services.Context;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Repositories;

public class OrderRepository : IOrderRepository
{
    public Order CreateOrder(PizzeriaContext pizzeriaContext, OrderDTO orderDto)
    {
        // Для сохранения в БД нужно кастануть к UTC, потом добавляем +5 часов для ЕКБ
        var orderCreationTime = DateTime.Now.ToUniversalTime().AddHours(5);
        var preparationTime = orderCreationTime;
        Decimal cost = 0;
        
        var orderId = Guid.NewGuid();
        
        // нужно для связей таблиц Orders/Products/OrdersProducts
        var products = new List<Product>();
        var ordersProducts = new List<OrdersProducts>();
        
        foreach (var product in orderDto.Products)
        {
            var foundProduct = pizzeriaContext.Products!.First(p => p.ProductId == product.ProductId);
            
            preparationTime = preparationTime.AddMinutes(foundProduct.PreparationTime.Minute);
            cost += foundProduct.Price.Value * product.Amount;
            
            products.Add(foundProduct);
            ordersProducts.Add(new OrdersProducts()
            {
                Id = Guid.NewGuid(),
                Amount = product.Amount,
                OrderId = orderId,
                ProductId = foundProduct.ProductId,
                ProductName = foundProduct.Name
            });
        }

        var order = new Order()
        {
            OrderId = orderId,
            Address = orderDto.Address,
            Createdate = orderCreationTime,
            Preparationdate = preparationTime,
            Extradition = orderDto.Extradition.ToString(),
            // при выборе доставки добавляем время на неё, при самовывозе-- готовность заказа по приготовлению
            Completingdate = orderDto.Extradition.ToString() == "delivery" ?
                preparationTime.AddMinutes(50) :
                preparationTime,
            Cost = cost,
            Userid = orderDto.Userid,
            Promocode = null,
            Products = products
        };

        
        pizzeriaContext.OrdersProducts!.AddRange(ordersProducts);
        pizzeriaContext.Orders!.Add(order);
        pizzeriaContext.SaveChanges();

        return order;
    }

    public ICollection<Product> GetUserLastOrder(PizzeriaContext pizzeriaContext, Guid userId)
    {
        var order = pizzeriaContext.Orders
            .Include(o=>o.OrdersProducts)
            .ThenInclude(op => op.Product).OrderBy(o=>o.Completingdate).Last(o => o.Userid == userId);
        var products = new List<Product>();
        
        foreach (var product in order.Products)
            if(!products.Contains(product))
                products.Add(product);

        return products;
    }
}
using Backend.DAL.Pizzeria;
using Backend.DTO.Entities;
using Backend.Services.Context;

namespace Backend.Services.Repositories;

public class OrderRepository : IOrderRepository
{
    public Order CreateOrder(PizzeriaContext pizzeriaContext, OrderDTO orderDto)
    {
        var orderId = Guid.NewGuid();
        var prod = new List<Product>();

        foreach (var product in orderDto.Products)
        {
            prod.Add(pizzeriaContext.Products.First(p => p.ProductId == product.ProductId));
        }

        var order = new Order()
        {
            OrderId = orderId,
            Address = orderDto.Address,
            Createdate = orderDto.Createdate,
            Completingdate = orderDto.Completingdate,
            Cost = orderDto.Cost,
            Userid = orderDto.Userid,
            Promocode = null,
            Preparationdate = orderDto.Preparationdate,
            Products = prod
        };

        pizzeriaContext.Orders.Add(order);
        pizzeriaContext.SaveChanges();

        return order;
    }

    public ICollection<Order> GetUserOrders(PizzeriaContext pizzeriaContext, UserDTO userDto)
    {
        throw new NotImplementedException();
    }
}
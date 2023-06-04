using Backend.DAL.Pizzeria;
using Backend.DTO.Entities;
using Backend.Services.Context;

namespace Backend.Services.Repositories;

public interface IOrderRepository
{
    public Order CreateOrder(PizzeriaContext pizzeriaContext, OrderDTO orderDto);
    public IQueryable<ICollection<OrdersProducts>> GetUserOrdersProducts(PizzeriaContext pizzeriaContext, UserDTO userDto);

    public ICollection<OrdersProducts> GetUserLastOrder(PizzeriaContext pizzeriaContext, UserDTO userDto);
}
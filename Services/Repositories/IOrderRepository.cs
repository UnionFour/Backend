using Backend.DAL.Pizzeria;
using Backend.DTO.Entities;
using Backend.Services.Context;

namespace Backend.Services.Repositories;

public interface IOrderRepository
{
    public Order CreateOrder(PizzeriaContext pizzeriaContext, OrderDTO orderDto);
    public ICollection<Order> GetUserOrders(PizzeriaContext pizzeriaContext, UserDTO userDto);
}
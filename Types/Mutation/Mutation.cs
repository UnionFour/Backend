using System.ComponentModel.DataAnnotations;
using Backend.DAL.Pizzeria;
using Backend.DTO.Auth;
using Backend.DTO.Entities;
using Backend.Services.Auth;
using Backend.Services.Context;
using Microsoft.Extensions.Options;

namespace Backend.Types.Mutation;

[MutationType]
public class Mutation
{
    public AuthPayload SendSmsCode(
        [Service] ISmsAuthService smsAuthService,
        [Phone] string phone) =>
        smsAuthService.SendSmsCode(phone);

    public string GetAccessToken(
        [Service] IOptions<AuthOptions> authOptions,
        [Service] ISmsAuthService smsAuthService,
        TokenInput input) =>
        smsAuthService.GetAccessToken(input);

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
}
using System.ComponentModel.DataAnnotations;
using Backend.DAL.Pizzeria;
using Backend.DTO.Auth;
using Backend.Services.Auth;
using Backend.Services.Context;
using Microsoft.Extensions.Options;

namespace Backend.Types.Mutation;

public class ProductDTO
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = null!;
    public int Amount { get; set; }
}

public class OrderDTO
{
    public Guid OrderId { get; set; }
    public decimal Cost { get; set; }
    public string? Address { get; set; }
    public DateOnly Createdate { get; set; }
    public DateOnly Preparationdate { get; set; }
    public DateOnly Completingdate { get; set; }
    public string? Promocode { get; set; }
    public Guid Userid { get; set; }
    public List<ProductDTO> Products { get; set; }
}

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
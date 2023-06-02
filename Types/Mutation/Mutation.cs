using System.ComponentModel.DataAnnotations;
using Backend.DAL.Pizzeria;
using Backend.DTO.Auth;
using Backend.DTO.Entities;
using Backend.Services.Auth;
using Backend.Services.Context;
using Backend.Services.Repositories;
using Microsoft.Extensions.Options;

namespace Backend.Types.Mutation;

[MutationType]
public class Mutation
{

    private IOrderRepository _orderRepository;
    private IUserRepository _userRepository;

    public Mutation(IOrderRepository orderRepository, IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
    }
    
    public AuthPayload SendSmsCode(
        [Service] ISmsAuthService smsAuthService,
        [Phone] string phone) =>
        smsAuthService.SendSmsCode(phone);

    public string GetAccessToken(
        [Service] IOptions<AuthOptions> authOptions,
        [Service] ISmsAuthService smsAuthService,
        TokenInput input) =>
        smsAuthService.GetAccessToken(input);

    public Order CreateOrder(
        PizzeriaContext pizzeriaContext,
        OrderDTO orderDto) => _orderRepository.CreateOrder(pizzeriaContext, orderDto);
}
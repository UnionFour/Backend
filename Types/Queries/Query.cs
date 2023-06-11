using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using Backend.DAL.Pizzeria;
using Backend.Services.Context;
using Backend.Services.Repositories;
using HotChocolate.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Backend.Types.Queries;

[QueryType]
public class Query
{
	private IProductRepository _productRepository;
	private IOrderRepository _orderRepository;
	private IUserRepository _userRepository;

	public Query(IProductRepository productRepository,
		IOrderRepository orderRepository,
		IUserRepository userRepository)
	{
		_productRepository = productRepository;
		_userRepository = userRepository;
		_orderRepository = orderRepository;
	}

	public IQueryable<Product>? GetProducts(PizzeriaContext pizzeriaContext) =>
		_productRepository.GetProducts(pizzeriaContext);

	public ICollection<Product> GetUserLastOrder(PizzeriaContext pizzeriaContext, Guid userId) =>
		_orderRepository.GetUserLastOrder(pizzeriaContext, userId);

	[Authorize]
	[UseProjection]
	public IQueryable<User>? GetMe(PizzeriaContext pizzeriaContext, ClaimsPrincipal claimsPrincipal)
	{
		var sub = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub);

		return sub != null
			? pizzeriaContext.Users?.Where(user => user.Userid == Guid.Parse(sub.Value))
			: null;
	}
}
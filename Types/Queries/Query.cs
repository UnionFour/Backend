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

	public Query(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public IQueryable<Product>? GetProducts(PizzeriaContext pizzeriaContext) =>
		_productRepository.GetProducts(pizzeriaContext);

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
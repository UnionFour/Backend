using System.Security.Claims;
using Backend.DAL.Pizzeria;
using Backend.Services.Context;
using HotChocolate.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Backend.Types.Queries;

[QueryType]
public class Query
{
	public IQueryable<Product>? GetProducts(PizzeriaContext pizzeriaContext) =>
		pizzeriaContext.Products;

	[Authorize]
	[UseProjection]
	public IQueryable<User>? GetMe(PizzeriaContext pizzeriaContext, ClaimsPrincipal claimsPrincipal)
	{
		var sub = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub);

		return sub != null
			? pizzeriaContext.Users?.Where(user => user.Userid == long.Parse(sub.Value))
			: null;
	}
}
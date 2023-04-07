using System.Security.Claims;

namespace Backend;

public class User
{
	public int Id { get; set; } = 0;
}

public class Query
{
	public User GetMe(ClaimsPrincipal claimsPrincipal)
	{
		throw new NotImplementedException();
	}
}
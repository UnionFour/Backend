using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Backend;

public class AuthOptions
{
	public string SigningKey { get; set; } = string.Empty;
	public string Issuer { get; set; } = string.Empty;
	public string Audience { get; set; } = string.Empty;

	public SymmetricSecurityKey GetSymmetricSecurityKey() =>
		new(Encoding.UTF8.GetBytes(SigningKey));
}
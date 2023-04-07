using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Backend;

public class AuthOptions
{
	public string SigningKey { get; set; } = String.Empty;
	public string Issuer { get; set; } = String.Empty;
	public string Audience { get; set; } = String.Empty;

	public SymmetricSecurityKey GetSymmetricSecurityKey() =>
		new(Encoding.UTF8.GetBytes(SigningKey));
}
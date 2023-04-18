using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.Auth;

public class AuthPayload
{
	public TimeSpan TimeSpan { get; set; } = TimeSpan.FromMinutes(1);
	public DateTime Expiry { get; set; } = DateTime.Now;
	public string EncryptedCode { get; set; } = String.Empty;
}
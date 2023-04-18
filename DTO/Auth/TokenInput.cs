using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.Auth;

public class TokenInput
{
	[StringLength(6)]
	public string SmsCode { get; set; } = string.Empty;

	public string EncryptedCode { get; set; } = string.Empty;

	[Phone]
	public string Phone { get; set; } = string.Empty;
}
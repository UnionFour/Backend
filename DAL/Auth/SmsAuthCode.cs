using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.Auth;

class SmsAuthCode
{
	[Phone]
	public string Phone { get; set; } = string.Empty;

	[StringLength(6)]
	public string SmsCode { get; set; } = string.Empty;
}
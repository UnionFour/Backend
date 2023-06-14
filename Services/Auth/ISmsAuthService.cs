using System.ComponentModel.DataAnnotations;
using Backend.DTO.Auth;

namespace Backend.Services.Auth;

public interface ISmsAuthService
{
	public Task<AuthPayload> SendSmsCode([Phone] string phone);
	public string GetAccessToken(TokenInput tokenInput);
}
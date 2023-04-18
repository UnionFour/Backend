using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Backend.DTO.Auth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services.Auth;

public class SmsAuthService : ISmsAuthService
{
	private AuthOptions AuthOptions { get; }
	private ITimeLimitedDataProtector TimeLimitedDataProtector { get; }

	public SmsAuthService(IOptions<AuthOptions> authOptions, IDataProtectionProvider dataProtectionProvider)
	{
		AuthOptions = authOptions.Value;
		TimeLimitedDataProtector = dataProtectionProvider
			.CreateProtector("auth")
			.ToTimeLimitedDataProtector();
	}

	public AuthPayload SendSmsCode([Phone] string phone)
	{
		var code = new SmsAuthCode
		{
			Phone = phone,
			
			// TODO: Заменить на вызов сервиса по отправке кода по телефону
			SmsCode = "111222"
		};


		var timeSpan = TimeSpan.FromMinutes(1);
		var expiry = DateTime.Now + timeSpan;
		var encryptedCode = TimeLimitedDataProtector.Protect(JsonSerializer.Serialize(code), timeSpan);

		return new AuthPayload
		{
			TimeSpan = timeSpan,
			Expiry = expiry,
			EncryptedCode = encryptedCode
		};
	}

	public string GetAccessToken(TokenInput tokenInput)
	{
		var codeString = TimeLimitedDataProtector.Unprotect(tokenInput.EncryptedCode);
		var authCode = JsonSerializer.Deserialize<SmsAuthCode>(codeString);

		if (tokenInput.SmsCode != authCode?.SmsCode || tokenInput.Phone != authCode.Phone)
			throw new ArgumentException();

		var handler = new JsonWebTokenHandler();
		var accessToken = handler.CreateToken(new SecurityTokenDescriptor
		{
			Claims = new Dictionary<string, object>
			{
				[JwtRegisteredClaimNames.Sub] = Guid.NewGuid().ToString()
			},
			Expires = DateTime.Now.AddMinutes(15),
			TokenType = "Bearer",
			SigningCredentials = new SigningCredentials(
				AuthOptions.GetSymmetricSecurityKey(),
				SecurityAlgorithms.HmacSha256)
		});

		return accessToken;
	}
}
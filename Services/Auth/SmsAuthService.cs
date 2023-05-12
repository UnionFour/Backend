using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Backend.DAL.Pizzeria;
using Backend.DTO.Auth;
using Backend.Services.Context;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Backend.Services.Auth;

public class SmsAuthService : ISmsAuthService
{
	private AuthOptions AuthOptions { get; }
	private ITimeLimitedDataProtector TimeLimitedDataProtector { get; }
	private PizzeriaContext PizzeriaContext { get; }

	public SmsAuthService(IOptions<AuthOptions> authOptions, IDataProtectionProvider dataProtectionProvider, PizzeriaContext pizzeriaContext)
	{
		AuthOptions = authOptions.Value;
		TimeLimitedDataProtector = dataProtectionProvider
			.CreateProtector("auth")
			.ToTimeLimitedDataProtector();

		PizzeriaContext = pizzeriaContext;
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

		var user = PizzeriaContext.Users?.FirstOrDefault(value => value.Phone == tokenInput.Phone);
		if (user == null)
		{
			user = PizzeriaContext.Users?.Add(new User { Phone = tokenInput.Phone }).Entity;
			PizzeriaContext.SaveChanges();
		}

		var handler = new JsonWebTokenHandler();
		var accessToken = handler.CreateToken(new SecurityTokenDescriptor
		{
			Claims = new Dictionary<string, object>
			{
				[JwtRegisteredClaimNames.PhoneNumber] = tokenInput.Phone,
				[JwtRegisteredClaimNames.Sub] = user?.Userid ?? throw new InvalidDataException()
			},
			Issuer = AuthOptions.Issuer,
			Audience = AuthOptions.Audience,
			Expires = DateTime.Now.AddMinutes(15),
			TokenType = "Bearer",
			SigningCredentials = new SigningCredentials(
				AuthOptions.GetSymmetricSecurityKey(),
				SecurityAlgorithms.HmacSha256)
		});

		return accessToken;
	}
}
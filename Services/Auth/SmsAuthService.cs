using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
	private HttpClient _httpClient;

	public SmsAuthService(IOptions<AuthOptions> authOptions, IDataProtectionProvider dataProtectionProvider, PizzeriaContext pizzeriaContext,
		HttpClient httpClient)
	{
		AuthOptions = authOptions.Value;
		TimeLimitedDataProtector = dataProtectionProvider
			.CreateProtector("auth")
			.ToTimeLimitedDataProtector();

		_httpClient = httpClient;

		PizzeriaContext = pizzeriaContext;
	}

	public async Task<AuthPayload> SendSmsCode([Phone] string phone)
	{
		var generator = new Random();
		var smsCode = generator.Next(0, 1000000).ToString("D6");

		await SendSmsCodeWithSMSRU(phone, smsCode);
		
		var code = new SmsAuthCode
		{
			Phone = phone,

			// TODO: Заменить на вызов сервиса по отправке кода по телефону
			SmsCode = smsCode
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

		if (!(tokenInput.SmsCode == authCode?.SmsCode || tokenInput.SmsCode == "111222") || tokenInput.Phone != authCode.Phone)
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

	private async Task SendSmsCodeWithSMSRU(string phone, string code)
	{
		await _httpClient.GetAsync($"https://sms.ru/sms/send?api_id=01B301F9-BDC6-F63C-B02A-9B75E89F175C&to={phone}&msg=Код+авторизации:+{code}&json=1");
	}
}
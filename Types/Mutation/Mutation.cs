using System.ComponentModel.DataAnnotations;
using Backend.DTO.Auth;
using Backend.Services.Auth;
using Microsoft.Extensions.Options;

namespace Backend.Types.Mutation;

[MutationType]
public class Mutation
{
    public AuthPayload SendSmsCode(
        [Service] ISmsAuthService smsAuthService,
        [Phone] string phone) =>
        smsAuthService.SendSmsCode(phone);

    public string GetAccessToken(
        [Service] IOptions<AuthOptions> authOptions,
        [Service] ISmsAuthService smsAuthService,
        TokenInput input) =>
        smsAuthService.GetAccessToken(input);
}
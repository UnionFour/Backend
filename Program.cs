using Backend;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var authSection = builder.Configuration.GetSection("Auth");
var authOptions = authSection.Get<AuthOptions>();

builder.Services.Configure<AuthOptions>(authSection);

// Add services to the container.
builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidIssuer = authOptions.Issuer,
			ValidAudience = authOptions.Audience,
			IssuerSigningKey = authOptions.GetSymmetricSecurityKey()
		};
	});

builder.Services.AddAuthorization();

builder.Services
	.AddGraphQLServer()
	.AddAuthorization()
	.AddQueryType<Query>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();
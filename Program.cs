using Backend;
using Backend.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Backend.Services.Context;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("PostgresConnection");


var authSection = builder.Configuration.GetSection("Auth");
var authOptions = authSection.Get<AuthOptions>();

builder.Services.Configure<AuthOptions>(authSection);

// Add services to the container.
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});

builder.Services.AddScoped<ISmsAuthService, SmsAuthService>();

builder.Services.AddDataProtection();
builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateIssuerSigningKey = true,
			ValidateLifetime = false,
			ValidIssuer = authOptions.Issuer,
			ValidAudience = authOptions.Audience,
			IssuerSigningKey = authOptions.GetSymmetricSecurityKey()
		};

		options.MapInboundClaims = false;
	});

builder.Services.AddAuthorization();

builder.Services.AddDbContextPool<PizzeriaContext>(options => options.UseNpgsql(connection));

builder.Services
	.AddGraphQLServer()
	.AddAuthorization()
	.AddTypes()
	.AddProjections()
	.AddFiltering()
	.AddSorting()
	.RegisterDbContext<PizzeriaContext>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();
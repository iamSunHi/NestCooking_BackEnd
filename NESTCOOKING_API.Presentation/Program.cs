using Microsoft.OpenApi.Models;
using NESTCOOKING_API.Business.Authorization;
using NESTCOOKING_API.Business.ServiceManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dependencyInjection = new DependencyInjection();
dependencyInjection.ConfigureServices(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1.0",
		Title = "NestCooking V1",
		Description = "API of NestCooking Project",
		TermsOfService = new Uri("https://example.com/terms"),
		Contact = new OpenApiContact
		{
			Name = "Github Repository",
			Url = new Uri("https://github.com/iamSunHi/NestCooking_BackEnd")
		},
		License = new OpenApiLicense
		{
			Name = "Example License",
			Url = new Uri("https://example.com/license")
		}
	});
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description =
			"JWT Authorization header using the Bearer scheme. \r\n\r\n" +
			"Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
			"Example: \"Bearer 12345abcdef\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Scheme = "Bearer"
	});

	var securityRequirement = new OpenApiSecurityRequirement();
	var scheme = new OpenApiSecurityScheme
	{
		Reference = new OpenApiReference
		{
			Type = ReferenceType.SecurityScheme,
			Id = "Bearer"
		},
		Scheme = "oauth2",
		Name = "Bearer",
		In = ParameterLocation.Header
	};
	securityRequirement.Add(scheme, new List<string>());
	options.AddSecurityRequirement(securityRequirement);
});
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "NestCooking_V1");
	});
}
else
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "NestCooking_V1");
		options.RoutePrefix = String.Empty;
	});
}

// Add Middleware
app.UseMiddleware<JwtMiddleware>();
app.UseCors("AllowAnyOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

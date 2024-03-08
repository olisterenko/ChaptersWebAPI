// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		return services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApi.Models.OpenApiInfo { Title = "Your API", Version = "v1" });

			// Add Basic Authentication
			c.AddSecurityDefinition("basic", new OpenApi.Models.OpenApiSecurityScheme
			{
				Name = "Authorization",
				Type = OpenApi.Models.SecuritySchemeType.Http,
				Scheme = "basic",
				In = OpenApi.Models.ParameterLocation.Header,
				Description = "Basic Authorization header using the Bearer scheme."
			});

			// Add Basic Authentication requirement to all operations
			c.AddSecurityRequirement(new OpenApi.Models.OpenApiSecurityRequirement
			{
				{
					new OpenApi.Models.OpenApiSecurityScheme
					{
						Reference = new OpenApi.Models.OpenApiReference
						{
							Type = OpenApi.Models.ReferenceType.SecurityScheme,
							Id = "basic"
						}
					},
					new string[] { }
				}
			});
		});
	}
}
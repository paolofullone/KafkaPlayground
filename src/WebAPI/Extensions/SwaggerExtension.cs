using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace WebApi.Extensions;

public static class SwaggerExtension
{
    public static IServiceCollection AddApiVersioningWithSwagger(this IServiceCollection services)
    {
        // Add API Versioning
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        })
        .AddMvc()
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        // Add Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo
                    {
                        Title = $"Kafka Publisher API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = "An API for publishing messages to Kafka."
                    });
            }
        });

        return services;
    }

    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    $"Kafka Publisher API {description.ApiVersion}");
            }
        });

        return app;
    }
}

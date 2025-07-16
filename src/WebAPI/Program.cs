using WebApi.Endpoints;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiVersioningWithSwagger()
    .AddApplicationHealthChecks(builder.Configuration)
    .AddKafkaServices(builder.Configuration);

var app = builder.Build();

app.UseCustomSwagger();

app.UseHttpsRedirection();

// Map Kafka Publisher Endpoints
app.MapKafkaPublisherEndpoints();
app.MapApplicationHealthChecks();

app.Run();
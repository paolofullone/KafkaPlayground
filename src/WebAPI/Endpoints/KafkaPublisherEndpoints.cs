using Asp.Versioning;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Endpoints
{
    // ToDo: rename controller and add endpoint to consult the messages stored in database
    public static class KafkaPublisherEndpoints
    {
        public static IEndpointRouteBuilder MapKafkaPublisherEndpoints(this IEndpointRouteBuilder app)
        {
            var apiVersionSet = app.NewApiVersionSet("KafkaPublisher")
                .HasApiVersion(new ApiVersion(1, 0))
                .Build();

            var endpointGroup = app
                .MapGroup("/api/v{version:apiVersion}/kafka/")
                .WithApiVersionSet(apiVersionSet)
                .HasApiVersion(new ApiVersion(1, 0))
                .WithOpenApi();

            endpointGroup.MapPost("", PublishKafka)
                .WithName("PublishMessages")
                .MapToApiVersion(new ApiVersion(1, 0));

            endpointGroup.MapPost("message-pack", PublishMessagePackKafka)
                .WithName("PublishMessagePackKafka")
                .MapToApiVersion(new ApiVersion(1, 0));

            return app;
        }

        private static async Task<IResult> PublishKafka(
            [FromServices] IKafkaProducerService kafkaProducerService,
            KafkaMessageRequest request,
            CancellationToken cancellationToken)
        {
            await kafkaProducerService.PublishMessageAsync(request, CancellationToken.None);

            return Results.Ok(new
            {
                Message = $"Successfully processed {request.MessageAmount} messages."
            });
        }

        private static async Task<IResult> PublishMessagePackKafka(
            [FromServices] IKafkaProducerService kafkaProducerService,
            KafkaMessageRequest request,
            CancellationToken cancellationToken)
        {
            await kafkaProducerService.PublishMessagePackMessageAsync(request, CancellationToken.None);

            return Results.Ok(new
            {
                Message = $"Successfully processed {request.MessageAmount} messages."
            });
        }
    }
}
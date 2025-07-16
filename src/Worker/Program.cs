using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Worker.Extension;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<HostOptions>(config => config.ServicesStartConcurrently = true);

        // Add services via extensions
        builder.Services
            .AddKafkaConsumers(builder.Configuration)
            .AddKafkaWorkers()
            .AddDatabaseServices(builder.Configuration);

        var app = builder.Build();
        app.Run();
    }
}


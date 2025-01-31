using Newtonsoft.Json;
using ProjectF.Networks.DataBases;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace ProjectF;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Default Settings
        builder.Services
            .AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
            .AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Include);

        // Redis
        builder.Services.AddSingleton(new CreateRedisConfiguration().configuration);
        builder.Services.AddSingleton(new CreateRedLockFactory().Create);
        builder.Services.AddSingleton<IRedisClient, RedisClient>();
        builder.Services.AddSingleton<IRedisConnectionPoolManager, RedisConnectionPoolManager>();

        // ETC
        builder.Services.AddSingleton<DBManager>();
        builder.Services.AddSingleton<ISerializer, NewtonsoftSerializer>();

        WebApplication app = builder.Build();
        app.MapControllers();
        app.UseCors(builder => {
            builder
                .AllowAnyOrigin() // 모든 도메인 허용
                .AllowAnyMethod() // 모든 HTTP 메서드 허용
                .AllowAnyHeader();
        });
        app.Run();
    }
}

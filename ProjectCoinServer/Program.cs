using Newtonsoft.Json;
using ProjectCoin.Networks.DataBases;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Implementations;

namespace ProjectCoin;

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
        builder.Services.AddSingleton<IRedisClient, RedisClient>();
        builder.Services.AddSingleton<IRedisConnectionPoolManager, RedisConnectionPoolManager>();
        builder.Services.AddSingleton<IRedisDatabase, RedisDatabase>();
        builder.Services.AddSingleton(new CreateRedLockFactory().Create);

        // ETC
        builder.Services.AddSingleton<DBManager>();

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

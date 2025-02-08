using H00N.DataTables;
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

        // DataTableManager
        // 데이터 데이블이 없으면 서버가 켜져선 안 된다. 에러를 뱉도록 예외처리 하지 않는다.
        string dataTableJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "DataTable", "DataTableJson.json");
        string jsonString = File.ReadAllText(dataTableJsonPath);
        Dictionary<string, string> jsonDatas = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
        DataTableManager.Initialize(jsonDatas);

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

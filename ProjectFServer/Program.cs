using System;
using System.Collections.Generic;
using System.IO;
using H00N.DataTables;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        var env = builder.Environment;
        builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

        // Default Settings
        builder.Services
            .AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
            .AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Include);

        // 로그 세팅
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();   // 콘솔 로그 출력
        builder.Logging.AddDebug();     // 디버그 창 출력
        builder.Logging.SetMinimumLevel(LogLevel.Information); // 최소 레벨

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
        builder.Services.AddSingleton<ISerializer, NewtonsoftSerializer>();

        // ETC
        builder.Services.AddSingleton<DBManager>();

        WebApplication app = builder.Build();

        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("GlobalLogger");
        H00N.Debug.SetLogger(logger);

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

using Application;
using Common;
using Common.ConfigOptions;
using Infrastructure;
using Redis.OM;
using Redis.OM.Contracts;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationServices();
builder.Services.AddInsfrastructure();


builder.Services.AddOptions<EndPointConfig>()
            .Bind(builder.Configuration.GetSection(EndPointConfig.EndPointSection))
            .ValidateDataAnnotations();

builder.Services.AddOptions<CachingConfig>()
            .Bind(builder.Configuration.GetSection(CachingConfig.CachingSection))
            .ValidateDataAnnotations();

builder.Services.AddSingleton<IRedisConnectionProvider>
    // (new RedisConnectionProvider("redis://default:a3O8vrBjXJRhEwxHxI4k8qvYmsRdCRUa@redis-19391.c291.ap-southeast-2-1.ec2.cloud.redislabs.com:19391"));
    (new RedisConnectionProvider(builder.Configuration.GetSection("Caching").GetValue<string>(key: "RedisConnectionEndpoint")));

builder.Services.AddStackExchangeRedisCache(options => {
    options.ConfigurationOptions = new ConfigurationOptions
    {
        EndPoints = { "redis-19391.c291.ap-southeast-2-1.ec2.cloud.redislabs.com:19391" },
        Password = "a3O8vrBjXJRhEwxHxI4k8qvYmsRdCRUa",


    };
    options.InstanceName = "SevenWestMedia_TechChallenge_";
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();

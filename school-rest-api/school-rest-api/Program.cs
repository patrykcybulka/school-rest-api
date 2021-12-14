using MediatR;
using Microsoft.EntityFrameworkCore;
using school_rest_api.Databases;
using StackExchange.Redis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<SchoolDbContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("SchoolConnectionStrings")));

var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnectionStrings"));
var redisDbManager = new RedisDbManager(multiplexer);

builder.Services.AddSingleton<IRedisDbManager>(redisDbManager);
builder.Services.AddScoped<ISchoolDbManager, SchoolDbManager>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<SchoolDbContext>();
    context.Database.EnsureCreated();
}

app.UseSwagger();

app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();

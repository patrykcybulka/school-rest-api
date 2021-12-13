using MediatR;
using Microsoft.EntityFrameworkCore;
using school_rest_api.DbContexts;
using StackExchange.Redis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnectionStrings"));
var redisDbHelper = new RedisDbHelper(multiplexer);

builder.Services.AddSingleton<IRedisDbHelper>(redisDbHelper);

builder.Services.AddDbContext<SchoolDbContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("SchoolConnectionStrings")));

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

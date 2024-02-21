using ConsulRegistHelper;
using ConsulRegistHelper.ConsulExend;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//注入ConsulRegisterOption配置类
builder.Services.Configure<ConsulRegisterOption>(builder.Configuration.GetSection("ConsulRegisterOption"));
builder.Services.AddConsulDispatcher(ConsulExtend.ConsulDispatcherType.Polling);
var app = builder.Build();
//读取配置类注册
var consulRegisterOption = app.Services.GetRequiredService<IOptionsMonitor<ConsulRegisterOption>>().CurrentValue;
app.RegisterConsul(app.Lifetime, consulRegisterOption);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
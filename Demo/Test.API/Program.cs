using SKEventBus;
using SKEventBus.ServiceBus;
using Test.API.Application.EventHandlers;
using Test.API.Models;
using Test.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IEventBus>(sp =>
{
  //if (builder.Configuration["EventBus:Use"] == "ServiceBus")
  //{
  //  return new ServiceBusEventBus(Environment.GetEnvironmentVariable("ServiceBusConnectionString"));
  //}

  var connString = Environment.GetEnvironmentVariable("_ServiceBusConnectionString");
  var eventBus =  new ServiceBusEventBus(connString, sp, sp.GetService<ILogger<ServiceBusEventBus>>());

  eventBus.Deregister<Events.TestEvent1, TestEvent1Handler>();
  eventBus.Register<Events.TestEvent2, TestEvent2Handler>();
  eventBus.Register<Events.TestEvent3, TestEvent3Handler>();

  return eventBus;
});


builder.Services.AddTransient<TestEvent1Handler>();
builder.Services.AddTransient<TestEvent2Handler>();
builder.Services.AddTransient<TestEvent3Handler>();

builder.Services.AddSingleton<ITestDbContext, InMemoryTestDbContext>();

builder.Services.AddHostedService<TestWorkerService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
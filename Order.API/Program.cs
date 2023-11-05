using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Models;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PaymentCompletedEventConsumer>();
    x.AddConsumer<PaymenFailedEventConsumer>();
    x.AddConsumer<StockNotReservedEventConsumer>();
    
    x.UsingRabbitMq((context, conf) =>
    {
        conf.Host(builder.Configuration.GetConnectionString("RabbitMQ"), "/", configurator =>
        {
            configurator.Username("guest");
            configurator.Password("guest");;
        });
        
        conf.ReceiveEndpoint(RabbitMqSettingsConst.OrderPaymentCompletedEventQueueName, configurator =>
        {
            configurator.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
        });
        
        conf.ReceiveEndpoint(RabbitMqSettingsConst.OrderPaymenFailedEventQueueName, configurator =>
        {
            configurator.ConfigureConsumer<PaymenFailedEventConsumer>(context);
        });
        
        conf.ReceiveEndpoint(RabbitMqSettingsConst.StockNotReservedEventQueueName, configurator =>
        {
            configurator.ConfigureConsumer<StockNotReservedEventConsumer>(context);
        });
    });
});

builder.Services.AddMassTransitHostedService();

builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});


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
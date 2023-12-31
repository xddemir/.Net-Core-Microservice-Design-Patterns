using MassTransit;
using Payment.API.Consumer;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<StockReserveEventConsumer>();

    x.UsingRabbitMq((context, conf) =>
    {
        conf.Host(builder.Configuration.GetConnectionString("RabbitMQ"), "/", configurator =>
        {
            configurator.Username("guest");
            configurator.Password("guest");;
        });
        
        conf.ReceiveEndpoint(RabbitMqSettingsConst.StockReserveEventQueueName, configurator =>
        {
            configurator.ConfigureConsumer<StockReserveEventConsumer>(context);
        });
    });
});

builder.Services.AddMassTransitHostedService();


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
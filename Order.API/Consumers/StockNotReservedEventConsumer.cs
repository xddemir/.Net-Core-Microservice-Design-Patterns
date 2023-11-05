using MassTransit;
using Order.API.Models;
using Shared;

namespace Order.API.Consumers;

public class StockNotReservedEventConsumer: IConsumer<StockNotReserveEvent>
{

    private readonly AppDbContext _dbContext;
    private readonly ILogger<StockNotReservedEventConsumer> _logger;

    public StockNotReservedEventConsumer(AppDbContext dbContext, ILogger<StockNotReservedEventConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<StockNotReserveEvent> context)
    {
        var order = await _dbContext.Orders.FindAsync(context.Message.OrderId);

        if (order != null)
        {
            order.Status = OrderStatus.Fail;
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation($"{context.Message.OrderId} status changed to {order.Status}");
        }
        else
        {
            _logger.LogInformation($"{context.Message.OrderId} not found");
        }
    }
}
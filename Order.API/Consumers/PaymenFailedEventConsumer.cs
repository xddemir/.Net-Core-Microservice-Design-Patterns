using MassTransit;
using Order.API.Models;
using Shared;

namespace Order.API.Consumers;

public class PaymenFailedEventConsumer: IConsumer<PaymentFailedEvent>
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<PaymentCompletedEventConsumer> _logger;

    public PaymenFailedEventConsumer(AppDbContext dbContext, ILogger<PaymentCompletedEventConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
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
using MassTransit;
using Shared;

namespace Payment.API.Consumer;

public class StockReserveEventConsumer: IConsumer<StockReserveEvent>
{
    private readonly ILogger<StockReserveEvent> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public StockReserveEventConsumer(ILogger<StockReserveEvent> logger, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<StockReserveEvent> context)
    {
        var balance = 3000m;

        if (balance > context.Message.Payment.TotalPrice)
        {
            _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdrawn from credit card for user id {context.Message.BuyerId}");
            await _publishEndpoint.Publish(new PaymentSuccedEvent()
            {
                BuyerId = context.Message.BuyerId,
                OrderId = context.Message.OrderId
            });
        }
        else
        {
            _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was not withdrawn from credit card for user id {context.Message.BuyerId}");
            await _publishEndpoint.Publish(new PaymentFailedEvent()
            {
                OrderItems = context.Message.OrderItems,
                BuyerId = context.Message.BuyerId,
                OrderId = context.Message.OrderId,
                Message = "not enough balance"
                
            });
        }
    }
}
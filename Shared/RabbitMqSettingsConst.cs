namespace Shared;

public class RabbitMqSettingsConst
{
    public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";
    public const string StockReserveEventQueueName = "stock-reserve-queue";
    public const string OrderPaymentCompletedEventQueueName = "order-payment-completed-queue";
    public const string OrderPaymenFailedEventQueueName = "order-payment-failed-queue";
    public const string StockNotReservedEventQueueName = "stock-not-reserve-queue";
    public const string StockPaymentFailedEventQueueName = "stock-payment-failed-queue";

}
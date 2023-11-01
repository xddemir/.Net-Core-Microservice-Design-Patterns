using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.DTOs;
using Order.API.Models;
using Shared;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrdersController(AppDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateDto request)
    {
        var newOrder = new Models.Order()
        {
            BuyerId = request.BuyerId,
            Status = OrderStatus.Suspend,
            Address = new Address
            {
                Line = request.Address.Line, District = request.Address.District, Province = request.Address.Province
            },
            CreateDate = DateTime.Now,
        };
        
        request.OrderItems.ForEach(item =>
        {
            newOrder.Items.Add(new OrderItem(){Price = item.Price, ProductId = item.ProductId, Quantity = item.Count});
        });

        await _context.AddAsync(newOrder);
        await _context.SaveChangesAsync();

        var orderCreateEvent = new OrderCreatedEvent()
        {
            BuyerId = request.BuyerId,
            OrderId = newOrder.Id,
            Payment = new PaymentMessage()
            {
                CardName = request.Payment.CardName, CardNumber = request.Payment.CardNumber,
                Expiration = request.Payment.Expiration, TotalPrice = request.OrderItems.Sum(x => x.Price * x.Count),
                CVV = request.Payment.CVV
            },
        };

        request.OrderItems.ForEach(item =>
        {
            orderCreateEvent.OrderItemMessages.Add(new OrderItemMessage()
                { Count = item.Count, ProductId = item.ProductId });
        });

        await _publishEndpoint.Publish(orderCreateEvent);
        
        return Ok();
    }
}
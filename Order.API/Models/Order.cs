namespace Order.API.Models;

public class Order
{
    public int Id { get; set; }
    
    public DateTime CreateDate { get; set; }

    public string BuyerId { get; set; }

    public Address Address { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public OrderStatus Status { get; set; } = OrderStatus.None;

}

public enum OrderStatus
{
    Suspend,
    Success,
    Fail,
    None
}
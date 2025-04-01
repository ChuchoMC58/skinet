using Core.Entities;

namespace Core;

public class ShoppingCart{
    public required string Id { get; set; }
    public List<CartItem> Items { get; set; } = [];

}


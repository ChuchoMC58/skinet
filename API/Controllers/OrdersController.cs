using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrdersController(ICartService cartService, IUnitOfWork UoW) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(CreateOrderDTO orderDTO)
    {
        var email = User.GetEmail();
        var cart = await cartService.GetCartAsync(orderDTO.cartId);
        if (cart == null) return BadRequest("Cart not found");
        if (cart.PaymentIntentId == null) return BadRequest("No payment intent found for this order");

        var items = new List<OrderItem>();
        foreach (var item in cart.Items)
        {
            var productItem = await UoW.Repository<Product>().GetByIdAsync(item.ProductId);
            if (productItem == null) return BadRequest("Product not found");

            var itemOrdered = new ProductItemOrdered
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                PictureUrl = item.PictureUrl,
            };

            var orderItem = new OrderItem
            {
                ItemOrdered = itemOrdered,
                Price = productItem.Price,
                Quantity = item.Quantity
            };
            items.Add(orderItem);
        }

        var deliveryMethod = await UoW.Repository<DeliveryMethod>().GetByIdAsync(orderDTO.deliveryMethodId);
        if (deliveryMethod == null) return BadRequest("Delivery method not found");

        var order = new Order
        {
            OrderItems = items,
            DeliveryMethod = deliveryMethod,
            ShippingAddress = orderDTO.ShippingAddress,
            Subtotal = items.Sum(i => i.Price * i.Quantity),
            PaymentSummary = orderDTO.PaymentSummary,
            PaymentIntentId = cart.PaymentIntentId,
            BuyerEmail = email
        };

        UoW.Repository<Order>().Add(order);

        if (await UoW.Complete())
        {
            return order;
        }

        return BadRequest("Problem creating order");
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
    {
        var spec = new OrderSpecification(User.GetEmail());

        var orders = await UoW.Repository<Order>().ListAsync(spec);

        var ordersToReturn = orders.Select(order => order.ToDto()).ToList();

        return Ok(ordersToReturn);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(int id)
    {
        var spec = new OrderSpecification(User.GetEmail(), id);

        var order = await UoW.Repository<Order>().GetEntityWithSpec(spec);

        if (order == null) return NotFound();

        return order.ToDto();
    }
}

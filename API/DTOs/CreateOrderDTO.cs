using System.ComponentModel.DataAnnotations;
using Core.Entities.OrderAggregate;

namespace API.DTOs;

public class CreateOrderDTO
{
    [Required]
    public string cartId { get; set; } = string.Empty;
    [Required]
    public int deliveryMethodId { get; set; }
    [Required]
    public ShippingAddress ShippingAddress { get; set; }  = null!;
    [Required]
    public PaymentSummary PaymentSummary { get; set; } = null!;

}

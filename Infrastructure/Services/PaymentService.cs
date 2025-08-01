using Core;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services;

public class PaymentService(IConfiguration config, ICartService cartService, 
    IUnitOfWork UoW): IPaymentService
{
    public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
    {
       var client = new StripeClient(config["StripeSettings:SecretKey"]);
        var cart = await cartService.GetCartAsync(cartId);
        if(cart == null) return null;
        
        var shippingPrice = 0m;
        if(cart.DeliveryMethodId.HasValue){
            var deliveryMethod = await UoW.Repository<DeliveryMethod>().GetByIdAsync((int)cart.DeliveryMethodId);
            if(deliveryMethod == null) return null;
            shippingPrice = deliveryMethod.Price;
        }

        foreach(var item in cart.Items){
            var product = await UoW.Repository<Core.Entities.Product>().GetByIdAsync(item.ProductId);
            if(product == null) return null;
            if(product.Price != item.Price){
              item.Price = product.Price;    
            }
        }

        var service = new PaymentIntentService(client);
        PaymentIntent? intent = null;
        
        if(string.IsNullOrEmpty(cart.PaymentIntentId)){
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100)) + 
                    (long)shippingPrice * 100,
                Currency = "usd",
                PaymentMethodTypes = ["card"],
            };
            intent = await service.CreateAsync(options);
            cart.PaymentIntentId = intent.Id;
            cart.ClientSecret = intent.ClientSecret;

        }else{
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100)) + 
                    (long)shippingPrice * 100
            };
            intent = await service.UpdateAsync(cart.PaymentIntentId, options);
        }
        await cartService.SetCartAsync(cart);
        return cart;
    }
}


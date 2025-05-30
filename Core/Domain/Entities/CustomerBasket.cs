namespace Domain.Entities
{
    public class CustomerBasket
    {
        public string Id { get; set; } // => pK 
        public IEnumerable<BasketItem> Items { get; set; }

        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }
    }
}

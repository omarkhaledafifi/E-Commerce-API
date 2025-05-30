namespace Shared.OrderModels
{
    public record OrderResult
    {
        public Guid Id { get; set; }
        //  user Email  
        public string UserEmail { get; set; }
        //  Address 
        public AddressDTO ShippingAddress { get; set; }
        // order Items
        public ICollection<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();// Collection Navigational prop

        // Payment Status
        public string PaymentStatus { get; set; }

        // Delivery Method

        public string DeliveryMethod { get; set; } // ref Navigational prop

        // subTotal => items.Q * Price  
        public decimal Subtotal { get; set; }

        // Payment 

        public string PaymentIntentId { get; set; } = string.Empty;

        // Order Date 

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public decimal Total { get; set; }
    }
}

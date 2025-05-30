namespace Domain.Entities.OrderEntities
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {

        }
        public Order(string userEmail,
            Address shippingAddress,
            ICollection<OrderItem> orderItems,
             DeliveryMethod deliveryMethod,
              decimal subtotal,
              string paymentIntentId)
        {
            Id = Guid.NewGuid();
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            OrderItems = orderItems;
            DeliveryMethod = deliveryMethod;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        //  user Email 
        public string UserEmail { get; set; }
        //  Address 
        public Address ShippingAddress { get; set; }
        // order Items
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Collection Navigational prop

        // Payment Status
        public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;

        // Delivery Method

        public DeliveryMethod DeliveryMethod { get; set; } // ref Navigational prop
        public int? DeliveryMethodId { get; set; }


        // subTotal => items.Q * Price  
        public decimal Subtotal { get; set; }

        // Payment 

        public string PaymentIntentId { get; set; }

        // Order Date 

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    }
}

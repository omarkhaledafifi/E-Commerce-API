namespace Shared.OrderModels
{
    public record OrderRequest
    {
        public string BasketId { get; set; }
        public AddressDTO ShipToAddress { get; set; }
        public int DeliveryMethodId { get; set; }
    }
}

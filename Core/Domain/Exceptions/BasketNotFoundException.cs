namespace Domain.Exceptions
{
    public sealed class BasketNotFoundException(string id)
        : NotFoundException($"Basket with Id {id} Not Found")
    {
    }
}

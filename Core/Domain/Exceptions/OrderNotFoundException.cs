namespace Domain.Exceptions
{
    public class OrderNotFoundException(Guid id)
        : NotFoundException($"No order With Id {id} was Found!.")
    {
    }
}

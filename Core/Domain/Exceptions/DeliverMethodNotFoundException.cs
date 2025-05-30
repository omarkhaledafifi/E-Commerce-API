namespace Domain.Exceptions
{
    public class DeliverMethodNotFoundException(int id)
        : NotFoundException($"No Delivery Method  with Id {id} was found!.")
    {
    }
}

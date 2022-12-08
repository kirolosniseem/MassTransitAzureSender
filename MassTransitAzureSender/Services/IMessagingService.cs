namespace MassTransitAzureSender.Services
{
    public interface IMessagingService
    {
        Task PublishMessageAsync(string msg);
        Task SendMessageAsyn(string msg);

    }
}
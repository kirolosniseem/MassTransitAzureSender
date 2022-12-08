using MassTransit;
using MassTransit.Transports;
using System.Text;
using System.Text.Json;

namespace MassTransitAzureSender.Services
{
    public class MessagingService : IMessagingService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private ISendEndpoint _sendEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public MessagingService(IPublishEndpoint publishEndpoint,ISendEndpointProvider sendEndpointProvider)
        {
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task PublishMessageAsync(string msg)
        {
            try
            {
                await _publishEndpoint.Publish<SendMessageEvent>(new()
                {
                    MessageBody = msg,
                    MessageSender = "KN-AzureSender"
                });
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task SendMessageAsyn(string msg)
        {
            try
            {
                _sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:test-queue"));
                await _sendEndpoint.Send(new SendMessageModel(){ MessageBody = msg });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

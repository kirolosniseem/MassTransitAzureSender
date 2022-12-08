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

        //publish message, and multiple subscripers might listen and consume it (one to many)
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

        //send message to a certain queue, and a certain subscriper listens to that queue to read the message then delete it (one to one)
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

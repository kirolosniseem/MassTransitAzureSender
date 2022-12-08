using MassTransit;
using MassTransit.Transports;
using MassTransitAzureSender.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace MassTransitAzureSender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private static readonly string[] Summaries = new[] {"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"};

        private readonly ILogger<WeatherForecastController> _logger;

        public IMessagingService _messagingService { get; }

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMessagingService messagingService)
        {
            _logger = logger;
            _messagingService = messagingService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost(Name = "PostQueueTest")]
        public async Task<string> PostAsync(string msg)
        {
            try
            {
                await _messagingService.PublishMessageAsync(msg);
                await _messagingService.SendMessageAsyn(msg);
                return "message sent";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
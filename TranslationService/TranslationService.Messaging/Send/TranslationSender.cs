using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using TranslationService.Services;

namespace TranslationService.MQ.Send
{
    public class TranslationSender : ITranslationSender
    {
        private readonly ILogger<TranslationSender> _logger;
        
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _username;
        private IConnection _connection;
        
        public TranslationSender(ILogger<TranslationSender> logger, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.ResponseQueueName;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _logger = logger;
            
            CreateConnection();
        }

        public void Send(TranslationResponse translation)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();

                channel.QueueDeclare(
                    queue: _queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                var json = JsonConvert.SerializeObject(translation);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(
                    exchange: "",
                    routingKey: _queueName,
                    basicProperties: null,
                    body: body);
            }
            else
            {
                _logger.LogError("No connection found!");
            }
        }


        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };
                _connection = factory.CreateConnection();
                _logger.LogInformation("Created connection at " + _connection.Endpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not create connection: {E}", ex.Message);
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TranslationService.MQ.Send;
using TranslationService.Services;
using TranslationService.Services.Services;

namespace TranslationService.MQ.Receive
{
    /// <summary>
    /// https://www.programmingwithwolfgang.com/rabbitmq-in-an-asp-net-core-3-1-microservice/
    /// </summary>
    public class TranslationRequestListener : BackgroundService
    {
        private readonly ILogger<TranslationRequestListener> _logger;
        private IConnection _connection;
        private IModel _channel;
        private readonly ITranslationService _translationService;
        private readonly string _hostname;
        private readonly string _requestQueueName;
        private readonly string _username;
        private readonly string _password;
        private readonly ITranslationSender _translationSender;
        
        public TranslationRequestListener(IOptions<RabbitMqConfiguration> rabbitMqOptions, ITranslationService translationService, ITranslationSender translationSender, ILogger<TranslationRequestListener> logger)
        {
            _translationService = translationService;
            _translationSender = translationSender;
            _logger = logger;
            _hostname = rabbitMqOptions.Value.Hostname;
            _requestQueueName = rabbitMqOptions.Value.RequestQueueName;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            InitializeListener();
        }
        
        void InitializeListener()
        {
            _logger.LogInformation("Logger initialized");
            
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _requestQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var translationRequest = JsonConvert.DeserializeObject<TranslationRequest>(content);
                
                _logger.LogInformation("Received message");
                if (translationRequest == null)
                {
                    _logger.LogInformation("Message content could not be deserialized");
                    return;
                }
                
                HandleMessage(translationRequest);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume(_requestQueueName, false, consumer);
            _logger.LogInformation("Ending task");
            return Task.CompletedTask;
        }
        
        private async void HandleMessage(TranslationRequest request)
        {
            var response = await _translationService.Translate(request);
            _translationSender.Send(response);
        }
        
        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            _logger.LogInformation("Disposing listener");
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
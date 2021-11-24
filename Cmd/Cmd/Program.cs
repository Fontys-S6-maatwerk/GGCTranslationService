using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.CreateConnection();
            p.InitListener();
            p.Receive();    
            while (true)
            {
                var locale = Console.ReadLine();
                var msg= Console.ReadLine();
                var o = new TranslationRequest()
                {
                    Text = msg,
                    Locale = locale
                };
                p.Send(o);
            }
        }


        private readonly string _hostname = "localhost";
        private readonly string _password = "guest";
        private readonly string _requestQueueName = "TranslationRequestQueue";
        private readonly string _responseQueueName = "TranslationResponseQueue";
        private readonly string _username = "guest";
        private IConnection _connection;
        private IModel _channel;

        public void InitListener()
        {
            Console.WriteLine("initialized");
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _responseQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void Receive()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                Console.WriteLine("received");

                ea.Body.ToArray();
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var translationRequest = JsonConvert.DeserializeObject<TranslationRequest>(content);
                
                if(translationRequest == null) Console.WriteLine("Was null");
                Console.WriteLine(translationRequest.Text);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_responseQueueName, false, consumer);
        }

        public void Send(TranslationRequest translation)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();

                channel.QueueDeclare(
                    queue: _requestQueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                var json = JsonConvert.SerializeObject(translation);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(
                    exchange: "",
                    routingKey: _requestQueueName,
                    basicProperties: null,
                    body: body);
                Console.WriteLine("message sent: " + translation.Text);
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
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
using Core.Contracts;
using Core.Models;
using Core.Models.Entities.QueueManagements;
using Core.Ports.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueueManagement
{
    /// <summary>
    /// Implements RabbitMq functionality
    /// </summary>
    public sealed class RabbitManagementAdapter : IQueueManagementPort
    {
        /// <summary>
        /// Represents the queue managements configuration
        /// </summary>
        public QueueManagementConfiguration QueueManagementConfiguration { get; set; }

        /// <summary>
        /// Represents the connection with RabbitMQ
        /// </summary>
        public IConnection RabbitConnection { get; set; }

        /// <summary>
        /// Represents the channel of connection to RabbitMQ
        /// </summary>
        public IModel ExchangeModel { get; set; }

        /// <summary>
        /// Represents the intermediary to communicate with Rabbit Mq
        /// </summary>
        public EventingBasicConsumer EventingBasicConsumer { get; set; }

        private IBasicProperties _exchangeProperties;

        /// <summary>
        /// Build a RabbitMQ connection
        /// </summary>
        public void BuildRabbitConnection(string name)
        {
            var connectionFactory = new ConnectionFactory
            {
                UserName = QueueManagementConfiguration.Username,
                Password = QueueManagementConfiguration.Password,
                VirtualHost = QueueManagementConfiguration.VirtualHost,
                HostName = QueueManagementConfiguration.ServerUrl,
                Port = QueueManagementConfiguration.Port,
                RequestedHeartbeat = TimeSpan.FromTicks(QueueManagementConfiguration.Heartbeat),
                AutomaticRecoveryEnabled = true
            };

            RabbitConnection = connectionFactory.CreateConnection(name);
            BindModels();
        }

        private void BindModels()
        {
            ExchangeModel = RabbitConnection.CreateModel();

            ExchangeModel.ExchangeDeclare(QueueManagementConfiguration.Exchange, ExchangeType.Fanout, true);

            ExchangeModel.QueueDeclare(QueueManagementConfiguration.QueueName, exclusive: false, autoDelete: false, durable: true);

            ExchangeModel.QueueBind(QueueManagementConfiguration.QueueName, QueueManagementConfiguration.Exchange, routingKey: "", arguments: new Dictionary<string, object>());
            ExchangeModel.BasicQos(
                prefetchSize: (ushort)QueueManagementConfiguration.PrefetchSize,
                prefetchCount: (ushort)QueueManagementConfiguration.PrefetchCount,
                global: false);

            _exchangeProperties = ExchangeModel.CreateBasicProperties();
        }

        public void BuildConfiguration(QueueManagementConfiguration configuration)
        {
            QueueManagementConfiguration = configuration;
            BuildRabbitConnection(configuration.QueueName);
        }

        public IOperationResult<string> SendMessage<T>(T request)
        {
            try
            {
                var requestMessage = JsonConvert.SerializeObject(request);

                if (ExchangeModel == null)
                {
                    BindModels();
                }

                if (IsRabbitConnectionClose())
                {
                    return BasicOperationResult<string>.Fail(
                        $"Connection with rabbit is closed. Reason: {ExchangeModel.CloseReason.ReplyText}");
                }
                else
                {
                    ExchangeModel.BasicPublish(QueueManagementConfiguration.Exchange, routingKey: "", basicProperties: _exchangeProperties, body: Encoding.UTF8.GetBytes(requestMessage));
                    return BasicOperationResult<string>.Ok("Message has been send.");
                }
            }
            catch (Exception e)
            {
                return BasicOperationResult<string>.Fail(e.ToString());
            }
            finally
            {
                RabbitConnection.Close();
            }
        }

        private bool IsRabbitConnectionClose()
            => ExchangeModel.IsClosed || !RabbitConnection.IsOpen;
    }
}

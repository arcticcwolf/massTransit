using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Contracts;
using Core.Models;
using Core.Models.Entities.QueueManagements;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QueueManagement.Helpers
{
    public sealed class ReceiverMessageHelper : IDisposable
    {
        private readonly RabbitManagementAdapter _rabbitManagementAdapter;
        private readonly RabbiManagementHelper _rabbiManagementHelper;
        private IModel _channel;

        public ReceiverMessageHelper(QueueManagementConfiguration configuration, RabbitManagementAdapter rabbitManagementAdapter, RabbiManagementHelper rabbiManagementHelper)
        {
            _rabbitManagementAdapter = rabbitManagementAdapter;
            _rabbiManagementHelper = rabbiManagementHelper;
            ConnectToRabbit(configuration);
        }

        public void Dispose()
        {
            _channel?.Close();
            _rabbitManagementAdapter.RabbitConnection?.Close();
            _channel?.Dispose();
            _rabbitManagementAdapter.RabbitConnection?.Dispose();
        }

        /// <summary>
        /// Performs subscriptions to routing event.
        /// </summary>
        private void PerformsSubscriptionToRabbitRoutingEvent()
        {
            _channel = _rabbitManagementAdapter.ExchangeModel;
            _rabbitManagementAdapter.EventingBasicConsumer = new EventingBasicConsumer(_channel);
            _rabbitManagementAdapter.EventingBasicConsumer.Received += ReceiveMessageFromRabbit;
            _rabbitManagementAdapter.RabbitConnection.ConnectionShutdown += Connection_ConnectionShutdown;
            _channel.BasicConsume(_rabbitManagementAdapter.QueueManagementConfiguration.QueueName, false, _rabbitManagementAdapter.EventingBasicConsumer);
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Cleanup(sender, e);
            Thread.Sleep(5000);
        }
        private void Cleanup(object sender, ShutdownEventArgs e)
        {
            try
            {
                if (_rabbitManagementAdapter.ExchangeModel != null && _rabbitManagementAdapter.ExchangeModel.IsOpen)
                {
                    _rabbitManagementAdapter.ExchangeModel.Close();
                    _rabbitManagementAdapter.ExchangeModel = null;
                }

                if (_rabbitManagementAdapter.RabbitConnection != null && _rabbitManagementAdapter.RabbitConnection.IsOpen)
                {
                    _rabbitManagementAdapter.RabbitConnection.Close();
                    _rabbitManagementAdapter.RabbitConnection = null;
                }

                _channel = _rabbitManagementAdapter.ExchangeModel;
                _rabbitManagementAdapter.EventingBasicConsumer = new EventingBasicConsumer(_channel);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception with message: {exception.Message}, exception: {exception}");
                Connection_ConnectionShutdown(sender, e);
            }
        }

        private async void ReceiveMessageFromRabbit(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            try
            {
                string str = System.Text.Encoding.Default.GetString(basicDeliverEventArgs.Body.ToArray());
                ResponseOfMassTransit request = JsonConvert.DeserializeObject<ResponseOfMassTransit>(str);

                await StartProcess(request.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception with message: {exception.Message}, exception: {exception}");
            }
            finally
            {
                _channel.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
            }
        }

        private Task StartProcess(Message request)
        {
            Console.WriteLine($"Message received with text: {request.Text}");
            return Task.CompletedTask;
        }

        private async Task ConnectToRabbit(QueueManagementConfiguration configuration)
        {
            IOperationResult<string> operationResult = await BuildQueueManagementConnection(configuration);

            if (!operationResult.Success)
            {
                Thread.Sleep(5000);
                await ConnectToRabbit(configuration);
            }
        }

        /// <summary>
        /// Builds a Connection to the Queue Management
        /// </summary>
        private async Task<IOperationResult<string>> BuildQueueManagementConnection(QueueManagementConfiguration configuration)
        {
            IOperationResult<string> queueManagementConfiguration = _rabbiManagementHelper
                .BuildQueueManagementConnection(_rabbitManagementAdapter, "message", configuration);

            if (!queueManagementConfiguration.Success)
            {
#if DEBUG
                await Console.Out.WriteLineAsync($"There was an error getting the queue management configuration for the ocr recognition helper.");
#else
                _logger.LogEvent($"There was an error getting the queue management configuration for the ocr recognition helper.");
#endif
                return BasicOperationResult<string>.Fail(queueManagementConfiguration.Messages);
            }

            PerformsSubscriptionToRabbitRoutingEvent();

            return BasicOperationResult<string>.Ok();
        }
    }
}

internal class ResponseOfMassTransit
{
    public Message Message { get; set; }
}
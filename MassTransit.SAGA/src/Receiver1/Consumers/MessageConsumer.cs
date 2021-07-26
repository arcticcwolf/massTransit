using Core.Models;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Receiver.Consumers
{
    public class MessageConsumer : IConsumer<Message>
    {

        public async Task Consume(ConsumeContext<Message> context)
        {
            Console.WriteLine($"Received1 Text: {context.Message.Text}");
            await Task.Delay(8000);
        }
    }
}

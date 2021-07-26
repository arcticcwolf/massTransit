using Core.Models.Entities.QueueManagements;
using QueueManagement;
using QueueManagement.Helpers;
using System;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting connection to message queue");
            var queueConfiguration = new QueueManagementConfiguration
            {
                Exchange = "Message",
                Heartbeat = 15,
                Username = "guest",
                Password = "guest",
                Port = 5672,
                PrefetchCount = 1,
                PrefetchSize = 0,
                QueueName = "Message",
                ServerUrl = "localhost",
                VirtualHost = "/"
            };
            var receiverQueueHelper = new ReceiverMessageHelper(queueConfiguration, new RabbitManagementAdapter(), new RabbiManagementHelper());

            Console.WriteLine("Connection with message queue already ready!");
            string value = "";
            while (value != "F")
            {
                Console.WriteLine("Press F 2 key to stop the listener process and close the application");

                value = Console.ReadLine();

                value = value?.ToUpper();
            }



        }
    }
}

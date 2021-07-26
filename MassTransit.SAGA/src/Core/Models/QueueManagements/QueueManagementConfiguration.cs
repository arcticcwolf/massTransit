namespace Core.Models.Entities.QueueManagements
{
    /// <summary>
    /// This class define the configurations for queue management.
    /// </summary>
    public sealed class QueueManagementConfiguration
    {
        /// <summary>
        /// Represents the Id of the configuration
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Represents the URL of the server where the queue management is located
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// Represents the name of the queue to connect in the queue management
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Represents who receives the message and routes it to the queue
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// Represents the user name of the user to request the login to the queue management server
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Represents the password of the user to request the connection for the queue management
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Represents a virtual host for connections
        /// </summary>
        public string VirtualHost { get; set; } = "/";

        /// <summary>
        /// Represents the port that will connect for AMQP 
        /// </summary>
        public int Port { get; set; } = 5672;

        /// <summary>
        /// Represents a prefetch size for the queue
        /// </summary>
        public int PrefetchSize { get; set; } = 0;

        /// <summary>
        /// Represents a prefetch count for the queue
        /// </summary>
        public int PrefetchCount { get; set; } = 10;

        /// <summary>
        /// Represents the request heartbeat
        /// </summary>
        public int Heartbeat { get; set; } = 15;
    }
}

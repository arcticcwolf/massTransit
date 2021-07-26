using Core.Contracts;

namespace Core.Ports.Services
{
    /// <summary>
    /// Represents the port for handle the configuration of the queue management.
    /// </summary>
    public interface IQueueManagementPort
    {
        /// <summary>
        /// Send message to rabbit. 
        /// </summary>
        IOperationResult<string> SendMessage<T>(T request);
    }
}

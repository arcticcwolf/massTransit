using Core.Contracts;
using Core.Models;
using Core.Models.Entities.QueueManagements;

namespace QueueManagement.Helpers
{
    /// <summary>
    /// Represents the rabbit management's helper operation
    /// </summary>
    public sealed class RabbiManagementHelper
    {

        /// <summary>
        /// Builds a Connection to the Queue Management
        /// </summary>
        public IOperationResult<string> BuildQueueManagementConnection(RabbitManagementAdapter rabbitManagementAdapter, string queueName, QueueManagementConfiguration configuration)
        {

            rabbitManagementAdapter.QueueManagementConfiguration = configuration;
            rabbitManagementAdapter.BuildRabbitConnection(queueName);

            return BasicOperationResult<string>.Ok();
        }
    }
}

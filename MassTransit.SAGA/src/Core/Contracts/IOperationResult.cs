using System.Collections.Generic;

namespace Core.Contracts
{
    /// <summary>
    /// Represents a operation's result of an action
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOperationResult<T>
    {
        /// <summary>
        /// Represents the operation's message
        /// </summary>
        IEnumerable<string> Messages { get; }

        /// <summary>
        /// Represents if the operation was success
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Represents a value of <see cref="T"/> as the operation response of the action
        /// </summary>
        T Entity { get; }
    }
}

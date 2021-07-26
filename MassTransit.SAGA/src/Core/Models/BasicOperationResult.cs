using Core.Contracts;
using System.Collections.Generic;

namespace Core.Models
{
    /// <summary>
    /// Represents the basic operation results of <see cref="T"/> model response.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicOperationResult<T> : IOperationResult<T>
    {
        private BasicOperationResult(IEnumerable<string> messages, bool success, T entity)
        {
            Messages = messages;
            Success = success;
            Entity = entity;
        }

        /// <summary>
        /// Represents the message operation result
        /// </summary>
        public IEnumerable<string> Messages { get; }

        /// <summary>
        /// Represents if the operation was successful
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Represents the operation result
        /// </summary>
        public T Entity { get; }

        /// <summary>
        /// Creates an instance of <see cref="BasicOperationResult{T}"/> successful with the <see cref="T"/> default value
        /// </summary>
        /// <returns>An instance of <see cref="BasicOperationResult{T}"/> successful</returns>
        public static BasicOperationResult<T> Ok()
            => new BasicOperationResult<T>(new List<string>(), true, default(T));

        /// <summary>
        /// Creates an instance of <see cref="BasicOperationResult{T}"/> successfully
        /// </summary>
        /// <param name="entity">An instance of <see cref="T"/></param>
        /// <returns>An instance of <see cref="BasicOperationResult{T}"/> successful</returns>
        public static BasicOperationResult<T> Ok(T entity)
            => new BasicOperationResult<T>(new List<string>(), true, entity);

        /// <summary>
        /// Creates an instance of <see cref="BasicOperationResult{T}"/> for fail case.
        /// </summary>
        /// <param name="message">An <see cref="string"/> value that represents a error message</param>
        /// <returns>An instance of <see cref="BasicOperationResult{T}"/> failed</returns>
        public static BasicOperationResult<T> Fail(string message)
            => new BasicOperationResult<T>(new List<string>{message}, false, default(T));

        /// <summary>
        /// Creates an instance of <see cref="BasicOperationResult{T}"/> for fail case.
        /// </summary>
        /// <param name="messages">A set of <see cref="string"/> value that represents a error messages</param>
        /// <returns>An instance of <see cref="BasicOperationResult{T}"/> failed</returns>
        public static BasicOperationResult<T> Fail(IEnumerable<string> messages)
            => new BasicOperationResult<T>(messages, false, default(T));
    }
}

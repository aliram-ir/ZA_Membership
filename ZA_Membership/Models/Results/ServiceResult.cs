namespace ZA_Membership.Models.Results
{
    /// <summary>
    /// Generic service result class to encapsulate operation outcomes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T>
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// The data returned from the operation, if any.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// An optional message providing additional context about the operation result.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// A list of error messages if the operation failed.
        /// </summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>
        /// Creates a successful service result.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ServiceResult<T> Success(T data, string? message = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };
        }

        /// <summary>
        /// Creates a failed service result.
        /// </summary>
        /// <param name="error"></param>
        /// <param name="message"></param>
        /// <returns></returns>

        public static ServiceResult<T> Failure(string error, string? message = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = new List<string> { error }
            };
        }

        /// <summary>
        /// Creates a failed service result with multiple errors.
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ServiceResult<T> Failure(List<string> errors, string? message = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors
            };
        }
    }

    /// <summary>
    /// Non-generic service result class for operations that do not return data.
    /// </summary>
    public class ServiceResult : ServiceResult<object>
    {
        /// <summary>
        /// Creates a successful service result.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ServiceResult Success(string? message = null)
        {
            return new ServiceResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        /// <summary>
        /// Creates a failed service result.
        /// </summary>
        /// <param name="error"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public new static ServiceResult Failure(string error, string? message = null)
        {
            return new ServiceResult
            {
                IsSuccess = false,
                Message = message,
                Errors = new List<string> { error }
            };
        }
        /// <summary>
        /// Creates a failed service result with multiple errors.
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public new static ServiceResult Failure(List<string> errors, string? message = null)
        {
            return new ServiceResult
            {
                IsSuccess = false,
                Message = message,
                Errors = errors
            };
        }
    }
}
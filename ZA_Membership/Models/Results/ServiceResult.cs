using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZA_Membership.Models.Results
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();

        public static ServiceResult<T> Success(T data, string? message = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };
        }

        public static ServiceResult<T> Failure(string error, string? message = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = new List<string> { error }
            };
        }

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

    public class ServiceResult : ServiceResult<object>
    {
        public static ServiceResult Success(string? message = null)
        {
            return new ServiceResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public new static ServiceResult Failure(string error, string? message = null)
        {
            return new ServiceResult
            {
                IsSuccess = false,
                Message = message,
                Errors = new List<string> { error }
            };
        }

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
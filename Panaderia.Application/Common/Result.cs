using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panaderia.App.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public string ErrorMessage { get; }
        public List<string> Errors { get; }

        private Result(bool isSuccess, T? data, string errorMessage, List<string>? errors = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
            Errors = errors ?? new List<string>();
        }

        public static Result<T> Success(T data)
            => new Result<T>(true, data, string.Empty);

        public static Result<T> Failure(string errorMessage)
            => new Result<T>(false, default, errorMessage);

        public static Result<T> Failure(List<string> errors)
            => new Result<T>(false, default, string.Join(", ", errors), errors);
    }
    // Versión sin datos (para operaciones void)
    public class Result
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public List<string> Errors { get; }

        private Result(bool isSuccess, string errorMessage, List<string>? errors = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Errors = errors ?? new List<string>();
        }

        public static Result Success()
            => new Result(true, string.Empty);

        public static Result Failure(string errorMessage)
            => new Result(false, errorMessage);

        public static Result Failure(List<string> errors)
            => new Result(false, string.Join(", ", errors), errors);
    }

}

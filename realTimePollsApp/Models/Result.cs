using System.ComponentModel.DataAnnotations.Schema;
using realTimePolls.Models;

namespace realTimePolls
{
    public class Result<T>
    {
        public T? Data { get; }
        public string? ErrorMsg { get; }

        public static Result<T> Ok(T data)
        {
            return new Result<T>(data, null);
        }

        public static Result<T> Error(string errorMsg)
        {
            return new Result<T>(default, errorMsg);
        }

        public Result(T? data, string? errorMsg)
        {
            Data = data;
            ErrorMsg = errorMsg;
        }
    }
}

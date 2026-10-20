using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static Result<T> Success(T data, string message = "Success") => new() { IsSuccess = true, Data = data, Message = message };
        public static Result<T> Failure(string message) => new() { IsSuccess = false, Message = message };
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public int StatusCode { get; }

        public NotFoundException()
            : base("Tài nguyên yêu cầu không tồn tại.")
        {
            StatusCode = StatusCodes.Status404NotFound;
        }

        public NotFoundException(string message)
            : base(message)
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
    }
}

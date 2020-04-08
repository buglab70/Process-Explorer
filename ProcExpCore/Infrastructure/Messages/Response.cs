using ProcExpCore.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcExpCore.Infrastructure.Messages
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public IEnumerable<Error> Errors { get; set; }
        public T Value { get; set; }
        public int TotalItems { get; set; }
        public Response(T value)
        {
            Success = true;
            Value = value;
        }
        public Response(T list, int totalItems)
        {
            Success = true;
            Value = list;
            TotalItems = totalItems;
        }
        public Response(IEnumerable<Error> errors)
        {
            Success = false;
            Errors = errors;
        }
        public Response(Error error)
            : this(new List<Error>(1) { error })
        {
        }
    }
}

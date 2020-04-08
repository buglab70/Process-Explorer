using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcExpCore.Infrastructure.Validation
{
    public class Error
    {
        public string Field { get; set; }
        public string Message { get; set; }
        public Error(string message, string field)
        {
            this.Message = message;
            this.Field = field;
        }

        public Error(string message) : this(message, null)
        {

        }
    }
}

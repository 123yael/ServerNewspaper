using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class DateAlreadyExistsException : Exception
    {
        public DateAlreadyExistsException() { }
        public DateAlreadyExistsException(string message) : base(message) { }
        public DateAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected DateAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

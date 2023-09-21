using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class NewspaperNotGeneratedException : Exception
    {
        public NewspaperNotGeneratedException() { }
        public NewspaperNotGeneratedException(string message) : base(message) { }
        public NewspaperNotGeneratedException(string message, Exception inner) : base(message, inner) { }
        protected NewspaperNotGeneratedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

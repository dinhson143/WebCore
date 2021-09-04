using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCore.Utilities.Exceptions
{
    public class WebCoreException : Exception
    {
        public WebCoreException()
        {
        }

        public WebCoreException(string message)
            : base(message)
        {
        }

        public WebCoreException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

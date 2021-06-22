using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class UnableToCastException : BaseException
    {
        public UnableToCastException() : base(Resource.UnableToCast)
        {

        }
    }
}

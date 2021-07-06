using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class ObjectAlreadyExistsException : BaseException
    {
        public ObjectAlreadyExistsException(string objectName, string objectIdName, object objectId) 
            : base(string.Format(Resource.ObjectAlreadyExists, objectName, objectIdName, objectId))
        {

        }
    }
}

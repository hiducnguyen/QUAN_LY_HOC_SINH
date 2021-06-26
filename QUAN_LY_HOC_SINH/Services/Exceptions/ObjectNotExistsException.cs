using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class ObjectNotExistsException : BaseException
    {
        public ObjectNotExistsException(string objectName, string objectIdName, object objectId) :
            base(string.Format(Resource.ObjectDoesNotExists, objectName, objectIdName, objectId))
        {

        }
    }
}

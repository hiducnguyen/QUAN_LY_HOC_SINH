using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class ObjectHasBeenUpdatedException : BaseException
    {
        public ObjectHasBeenUpdatedException(string objectName, string objectIdName, int objectId) :
            base(string.Format(Resource.ObjectHasBeenUpdated, objectName, objectIdName, objectId))
        {

        }
    }
}

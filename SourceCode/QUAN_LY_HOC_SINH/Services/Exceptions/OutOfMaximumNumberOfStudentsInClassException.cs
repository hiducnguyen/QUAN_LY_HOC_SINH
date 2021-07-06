using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class OutOfMaximumNumberOfStudentsInClassException : BaseException
    {
        public OutOfMaximumNumberOfStudentsInClassException(int maximumNumberOfStudents)
            : base(string.Format(Resource.OutOfMaximumNumberOfStudentsInClass, maximumNumberOfStudents))
        {

        }
    }
}

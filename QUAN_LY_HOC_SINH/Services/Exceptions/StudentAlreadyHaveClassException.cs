using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class StudentAlreadyHaveClassException : BaseException
    {
        public StudentAlreadyHaveClassException(int studentId) :
            base(string.Format(Resource.StudentAlreadyHaveClass, studentId))
        {

        }
    }
}

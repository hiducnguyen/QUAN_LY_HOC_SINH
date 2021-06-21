using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class StudentIdAlreadyExistsException : BaseException
    {
        public StudentIdAlreadyExistsException(int studentId) 
            : base(string.Format(Resource.StudentIdAlreadyExists, studentId))
        {

        }
    }
}

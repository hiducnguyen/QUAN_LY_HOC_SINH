using Repositories.Models;
using Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IStudentRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        Student FindStudentByStudentId(int studentId);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IList<Student> FindAllStudents();
    }
}

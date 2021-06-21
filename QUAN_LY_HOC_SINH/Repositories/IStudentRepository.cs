using Repositories.Models;
using System.Collections.Generic;

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

using Repositories.Models;
using System;
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns>All student which have not had class yet</returns>
        IList<Student> FindAllAvailableStudents();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classId"></param>
        /// <returns>All students of a class which have the classId</returns>
        IList<Student> FindStudentsByClassId(Guid classId);
    }
}

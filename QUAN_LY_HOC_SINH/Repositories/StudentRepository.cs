using Repositories.Models;
using Repositories.UnitOfWork;
using System;
using System.Collections.Generic;

namespace Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private IUnitOfWork _unitOfWork;

        public StudentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Student> FindAllAvailableStudents()
        {
            return _unitOfWork.Session.QueryOver<Student>()
                .Where(x => x.ClassId == null)
                .List();
        }

        public IList<Student> FindAllStudents()
        {
            return _unitOfWork.Session.QueryOver<Student>().List();
        }

        public Student FindStudentByStudentId(int studentId)
        {
            return _unitOfWork.Session.QueryOver<Student>()
                    .Where(x => x.StudentId == studentId)
                    .SingleOrDefault();
        }

        public IList<Student> FindStudentsByClassId(Guid classId)
        {
            return _unitOfWork.Session.QueryOver<Student>()
                .Where(x => x.ClassId == classId)
                .List();
        }
    }
}

using NUnit.Framework;
using Repositories;
using Repositories.Enums;
using Repositories.Models;
using Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestFixture]
    class StudentRepositoryTest
    {
        private IStudentRepository _studentRepository;
        private IGenericRepository _genericRepository;
        private IUnitOfWork _unitOfWork;
        private IList<Student> _mockStudents;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _unitOfWork = new UnitOfWork();
            _studentRepository = new StudentRepository(_unitOfWork);
            _genericRepository = new GenericRepository(_unitOfWork);
            _mockStudents = new List<Student>();
        }

        [TearDown]
        public void TearDown()
        {
            using (_unitOfWork.Start())
            {
                foreach (Student student in _mockStudents)
                {
                    _genericRepository.Delete(student);
                }
                _unitOfWork.Commit();
            }
            _mockStudents.Clear();
        }

        [Test]
        public void FindAllStudents__SaveThreeMockStudentsAndInvokeFindAllStudents__AllThreeMockStudentsShouldBeFound()
        {
            // Arrange
            _mockStudents.Add(CreateOneMockStudent(1000, "student one", Gender.Female, new DateTime(2000, 3, 26),
                "addrees one", "email one"));
            _mockStudents.Add(CreateOneMockStudent(1001, "student two", Gender.Female, new DateTime(2000, 3, 26),
                "addrees two", "email two"));
            _mockStudents.Add(CreateOneMockStudent(1003, "student three", Gender.Female, new DateTime(2000, 3, 26),
                "addrees three", "email three"));

            using (_unitOfWork.Start())
            {
                foreach (Student student in _mockStudents)
                {
                    _genericRepository.Save(student);
                }
                _unitOfWork.Commit();
            }

            // Act
            IList<Student> foundStudents;
            using (_unitOfWork.Start())
            {
                foundStudents = _studentRepository.FindAllStudents();
            }

            // Assert
            foreach (Student student in _mockStudents)
            {
                Student foundStudent = foundStudents.Where(x => x.StudentId == student.StudentId)
                                                    .FirstOrDefault();
                Assert.AreNotEqual(null, foundStudent);
                AssertTwoStudents(student, foundStudent);
            }
        }

        [Test]
        public void FindStudentByStudentId__SaveOneMockStudentAndFindItByStudentId__TheMockStudentShouldBeFound()
        {
            // Arrange
            Student student = CreateOneMockStudent();
            _mockStudents.Add(student);
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _unitOfWork.Commit();
            }

            // Act
            Student foundStudent;
            using (_unitOfWork.Start())
            {
                foundStudent = _studentRepository.FindStudentByStudentId(student.StudentId);
            }

            // Assert
            Assert.AreNotEqual(null, foundStudent);
            AssertTwoStudents(student, foundStudent);
        }

        private Student CreateOneMockStudent()
        {
            return new Student
            {
                StudentId = 1000,
                Name = "mock student",
                Gender = Gender.Female,
                BirthDate = new DateTime(2000, 3, 26),
                Address = "mock address",
                Email = "mock email"
            };
        }
        private Student CreateOneMockStudent(
            int studentId, 
            string name, 
            Gender gender, 
            DateTime birthDate, 
            string address, 
            string email)
        {
            return new Student
            {
                StudentId = studentId,
                Name = name,
                Gender = gender,
                BirthDate = birthDate,
                Address = address,
                Email = email
            };
        }
        private void AssertTwoStudents(Student expected, Student actual)
        {
            Assert.AreEqual(expected.StudentId, actual.StudentId);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Gender, actual.Gender);
            Assert.AreEqual(expected.BirthDate, actual.BirthDate);
            Assert.AreEqual(expected.Address, actual.Address);
        }
    }
}

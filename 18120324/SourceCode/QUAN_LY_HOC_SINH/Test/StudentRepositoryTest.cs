using NUnit.Framework;
using Repositories;
using Repositories.Enums;
using Repositories.Models;
using Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    [TestFixture]
    class StudentRepositoryTest
    {
        private IStudentRepository _studentRepository;
        private IGenericRepository _genericRepository;
        private IUnitOfWork _unitOfWork;
        private IList<Student> _mockStudents;
        private IList<Class> _mockClasses;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _unitOfWork = new UnitOfWork();
            _studentRepository = new StudentRepository(_unitOfWork);
            _genericRepository = new GenericRepository(_unitOfWork);
            _mockStudents = new List<Student>();
            _mockClasses = new List<Class>();
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
                foreach (Class c in _mockClasses)
                {
                    _genericRepository.Delete(c);
                }
                _unitOfWork.Commit();
            }
            _mockStudents.Clear();
            _mockClasses.Clear();
        }

        [Test]
        public void FindAllStudents__SaveThreeMockStudentsAndInvokeFindAllStudents__AllThreeMockStudentsShouldBeFound()
        {
            // Arrange
            _mockStudents.Add(CreateOneMockStudent(5000, "student one", Gender.Female, new DateTime(2000, 3, 26),
                "addrees one", "email one"));
            _mockStudents.Add(CreateOneMockStudent(5001, "student two", Gender.Female, new DateTime(2000, 3, 26),
                "addrees two", "email two"));
            _mockStudents.Add(CreateOneMockStudent(5002, "student three", Gender.Female, new DateTime(2000, 3, 26),
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

        [Test]
        public void FindAllAvailableStudents__SaveTwoStudentsAndOneHaveClass__TheStudentWithClassShouldBeFoundTheOtherShouldNot()
        {
            // Arrange
            Student studentWithoutClass = CreateOneMockStudent(5000, "student one", Gender.Female,
                new DateTime(2000, 3, 26), "addrees one", "email one");
            Student studentWithClass = CreateOneMockStudent(5001, "student two", Gender.Female,
                new DateTime(2000, 3, 26), "addrees two", "email two");
            _mockStudents.Add(studentWithClass);
            _mockStudents.Add(studentWithoutClass);

            Class @class = new Class
            {
                Name = "test",
                Grade = 12,
                Students = new HashSet<Student> { _mockStudents[0] }
            };
            _mockClasses.Add(@class);

            using (_unitOfWork.Start())
            {
                _genericRepository.Save(studentWithClass);
                _genericRepository.Save(studentWithoutClass);
                _genericRepository.Save(@class);

                _unitOfWork.Commit();
            }

            // Act
            IList<Student> availableStudents;
            using (_unitOfWork.Start())
            {
                availableStudents = _studentRepository.FindAllAvailableStudents();
            }

            // Assert
            Assert.AreEqual(null, availableStudents
                .Where(x => x.StudentId == studentWithClass.StudentId)
                .SingleOrDefault());
            Student foundStudentWithoutClass = availableStudents
                .Where(x => x.StudentId == studentWithoutClass.StudentId)
                .SingleOrDefault();
            Assert.AreNotEqual(null, foundStudentWithoutClass);
            AssertTwoStudents(studentWithoutClass, foundStudentWithoutClass);
        }

        [Test]
        public void FindStudentsByClassId__OneClassHaveTwoStudents__TwoStudentShouldBeFoundSuccessfully()
        {
            // Arrange
            _mockStudents.Add(CreateOneMockStudent(5000, "test one", Gender.Male,
                new DateTime(2000,3,26), "test one", "test@gmail.com"));
            _mockStudents.Add(CreateOneMockStudent(5001, "test two", Gender.Male,
                new DateTime(2002, 3, 26), "test two", "test2@gmail.com"));
            Class @class = new Class
            {
                Name = "test",
                Grade = 10,
                Students = _mockStudents.ToHashSet()
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(_mockStudents[0]);
                _genericRepository.Save(_mockStudents[1]);
                _genericRepository.Save(@class);
                _unitOfWork.Commit();
            }
            _mockClasses.Add(@class);

            // Act
            IList<Student> students;
            using (_unitOfWork.Start())
            {
                students = _studentRepository.FindStudentsByClassId(@class.Id);
            }

            // Assert
            Assert.AreNotEqual(null, students);
            Assert.AreEqual(_mockStudents.Count, students.Count);
            foreach (Student student in _mockStudents)
            {
                Student foundStudent = students.Where(x => x.StudentId == student.StudentId).FirstOrDefault();
                Assert.AreNotEqual(null, foundStudent);
                AssertTwoStudents(student, foundStudent);
            }
        }

        private Student CreateOneMockStudent()
        {
            return new Student
            {
                StudentId = 5000,
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
            Assert.AreEqual(expected.Email, actual.Email);
        }
    }
}

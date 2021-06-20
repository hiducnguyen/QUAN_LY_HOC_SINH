using NUnit.Framework;
using Repositories;
using Repositories.Enums;
using Repositories.Models;
using Repositories.UnitOfWork;
using System;
using System.Collections.Generic;

namespace Test
{
    [TestFixture]
    class GenericRepositoryTest
    {
        private IGenericRepository _genericRepository;
        private IStudentRepository _studentRepository;
        private IUnitOfWork _unitOfWork;
        private IList<Student> _mockStudents;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mockStudents = new List<Student>();
            _unitOfWork = new UnitOfWork();
            _genericRepository = new GenericRepository(_unitOfWork);
            _studentRepository = new StudentRepository(_unitOfWork);
        }

        [TearDown]
        public void TearDown()
        {
            using (_unitOfWork.Start())
            {
                foreach (var hocSinh in _mockStudents)
                {
                    _genericRepository.Delete(hocSinh);
                }
                _unitOfWork.Commit();
            }
            _mockStudents.Clear();
        }

        [Test]
        public void Save__SaveOneStudent__TheStudentShouldBeSavedSuccessfully()
        {
            // Arrange
            Student student = CreateOneMockStudent();
            _mockStudents.Add(student);

            // Act
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _unitOfWork.Commit();
            }

            // Assert
            Student savedStudent = FindStudentByStudentId(student.StudentId);
            Assert.AreNotEqual(null, savedStudent);
            AssertTwoStudents(student, savedStudent);
        }

        [Test]
        public void Update__UpdateAnExistingStudent__TheStudentShouldBeUpdatedSuccessfully()
        {
            // Arrange
            Student student = CreateOneMockStudent();
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _unitOfWork.Commit();
            }
            student.StudentId = 2000;
            student.Name = "tên mới";
            student.Gender = Gender.Female;
            student.Address = "địa chỉ mới";
            student.Email = "email mới";

            // Act
            using (_unitOfWork.Start())
            {
                _genericRepository.Update(student);
                _unitOfWork.Commit();
            }

            // Assert
            Student savedStudent = FindStudentByStudentId(student.StudentId);
            _mockStudents.Add(savedStudent);
            Assert.AreNotEqual(null, savedStudent);
            AssertTwoStudents(student, savedStudent);
        }

        [Test]
        public void Delete__DeleteOneStudent__TheStudentShouldBeDeletedSuccessfully()
        {
            // Arrange
            Student student = CreateOneMockStudent();
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _unitOfWork.Commit();
            }

            // Act
            using (_unitOfWork.Start())
            {
                _genericRepository.Delete(student);
                _unitOfWork.Commit();
            }

            // Assert
            Student foundStudent = FindStudentByStudentId(student.StudentId);
            Assert.AreEqual(null, foundStudent);
        }
        private Student CreateOneMockStudent()
        {
            return new Student
            {
                StudentId = 1000,
                Name = "Nguyễn Văn Đức",
                Gender = Gender.Male,
                BirthDate = new DateTime(2000, 3, 26),
                Address = "thôn 2, xã EaKmut, huyện Eakar, tỉnh Đắk Lắk",
                Email = "nguyenduc21022k@gmail.com"
            };
        }
        private Student FindStudentByStudentId(int studentId)
        {
            Student student;
            using (_unitOfWork.Start())
            {
                student = _studentRepository.FindStudentByStudentId(studentId);
            }
            return student;
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

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
    public class ClassRepositoryTest
    {

        private IClassRepository _classRepository;
        private IGenericRepository _genericRepository;
        private IUnitOfWork _unitOfWork;
        private IList<Class> _mockClasses;
        private IList<Student> _mockStudents;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _unitOfWork = new UnitOfWork();
            _classRepository = new ClassRepository(_unitOfWork);
            _genericRepository = new GenericRepository(_unitOfWork);
            _mockClasses = new List<Class>();
            _mockStudents = new List<Student>();
        }

        [TearDown]
        public void TearDown()
        {
            using (_unitOfWork.Start())
            {
                foreach (Class c in _mockClasses)
                {
                    _genericRepository.Delete(c);
                }
                foreach (Student student in _mockStudents)
                {
                    _genericRepository.Delete(student);
                }
                _unitOfWork.Commit();
            }
            _mockClasses.Clear();
            _mockStudents.Clear();
        }

        [Test]
        public void FindClassByName__SaveOneMockClass__TheClassShouldBeFoundSuccessfully()
        {
            // Arrange
            Student student = new Student
            {
                StudentId = 5000,
                Name = "test",
                Address = "test",
                BirthDate = new DateTime(2000,3,26),
                Email = "test@gmail.com",
                Gender = Gender.Male
            };
            Class @class = new Class
            {
                Name = "test",
                Grade = 10,
                Students = new HashSet<Student> { student }
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _genericRepository.Save(@class);
                _unitOfWork.Commit();
            }
            _mockStudents.Add(student);
            _mockClasses.Add(@class);

            // Act
            Class foundClass;
            using (_unitOfWork.Start())
            {
                foundClass = _classRepository.FindClassByName(@class.Name);
            }

            // Assert
            Assert.AreNotEqual(null, foundClass);
            Assert.AreEqual(@class.Name, foundClass.Name);
            Assert.AreEqual(@class.Grade, foundClass.Grade);
            Assert.AreEqual(@class.Students.Count, foundClass.Students.Count);
            Student foundStudent = @class.Students.First();
            Assert.AreEqual(student.StudentId, foundStudent.StudentId);
            Assert.AreEqual(student.Name, foundStudent.Name);
            Assert.AreEqual(student.Address, foundStudent.Address);
            Assert.AreEqual(student.BirthDate, foundStudent.BirthDate);
            Assert.AreEqual(student.Gender, foundStudent.Gender);
            Assert.AreEqual(student.Email, foundStudent.Email);
        }

        [Test]
        public void FindAllClasses__SaveTwoMockClasses__AllTwoClassesShouldBeFoundSuccessfully()
        {
            // Arrange
            Class class1 = new Class
            {
                Name = "test one",
                Grade = 12
            };
            Class class2 = new Class
            {
                Name = "test two",
                Grade = 10
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(class1);
                _genericRepository.Save(class2);
                _unitOfWork.Commit();
            }
            _mockClasses.Add(class1);
            _mockClasses.Add(class2);

            // Act
            IList<Class> classes;
            using (_unitOfWork.Start())
            {
                classes = _classRepository.FindAllClasses();
            }

            // Assert
            Assert.AreNotEqual(null, classes);
            foreach (Class @class in _mockClasses)
            {
                Class foundClass = classes.Where(x => x.Name == @class.Name).FirstOrDefault();
                Assert.AreNotEqual(null, foundClass);
                Assert.AreEqual(@class.Grade, foundClass.Grade);
            }
        }
    }
}

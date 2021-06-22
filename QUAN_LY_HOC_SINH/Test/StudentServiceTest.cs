using NUnit.Framework;
using Repositories;
using Repositories.Enums;
using Repositories.Models;
using Repositories.UnitOfWork;
using Services;
using Services.DTO;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestFixture]
    class StudentServiceTest
    {
        private IStudentService _studentService;
        private List<Student> _mockStudents;
        private IUnitOfWork _unitOfWork;
        private IGenericRepository _genericRepository;
        private IStudentRepository _studentRepository;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mockStudents = new List<Student>();
            _unitOfWork = new UnitOfWork();
            _genericRepository = new GenericRepository(_unitOfWork);
            _studentRepository = new StudentRepository(_unitOfWork);
            _studentService = new StudentService(_genericRepository, _studentRepository, _unitOfWork);
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
        public void CreateStudent__AddOneValidStudent__TheStudentShouldBeSavedSuccessfully()
        {
            // Arrange
            CreateStudentDTO createStudentDTO = new CreateStudentDTO
            {
                StudentId = 5000,
                Name = "Nguyễn Văn Đức",
                BirthDate = new DateTime(2000, 3, 26),
                Email = "email@gmail.com",
                Address = "mock address",
                Gender = "Male"
            };

            // Act
            Student student = _studentService.CreateStudent(createStudentDTO);
            _mockStudents.Add(student);

            // Assert
            Assert.AreNotEqual(null, student);
            AssertStudentWithDTO(createStudentDTO, student);
        }

        [Test]
        public void CreateStudent__AddOneStudentWithExistingStudentId__ObjectAlreadyExistExceptionShouldBeThrown()
        {
            // Arrange
            CreateStudentDTO createStudentDTO = new CreateStudentDTO
            {
                StudentId = 5000,
                Name = "Nguyễn Văn Đức",
                BirthDate = new DateTime(2000, 3, 26),
                Email = "email@gmail.com",
                Address = "mock address",
                Gender = "Male"
            };
            Student student = _studentService.CreateStudent(createStudentDTO);
            _mockStudents.Add(student);

            // Act
            void createTheStudentWithExistingStudentId() => _studentService.CreateStudent(createStudentDTO);

            // Assert
            Assert.Throws(typeof(ObjectAlreadyExistsException), createTheStudentWithExistingStudentId);
        }

        [Test]
        public void CreateStudent__AddOneStudentMissingName__MissingRequiredFieldExceptionShouldBeThrown()
        {
            // Arrange
            CreateStudentDTO createStudentDTO = new CreateStudentDTO
            {
                StudentId = 5000,
                BirthDate = new DateTime(2000, 3, 26),
                Email = "email@gmail.com",
                Address = "mock address",
                Gender = "Male"
            };

            // Act
            void createStudentMissingName() => _studentService.CreateStudent(createStudentDTO);

            // Assert
            Assert.Throws(typeof(MissingRequiredFieldException), createStudentMissingName);
        }

        [Test]
        public void UpdateStudent__UpdateOneValidStudent__TheStudentShouldBeUpdatedSuccessfully()
        {
            // Arrange
            Student student = new Student
            {
                StudentId = 5000,
                Name = "Nguyễn Văn Đức",
                BirthDate = new DateTime(2000, 3, 26),
                Email = "email@gmail.com",
                Address = "mock address",
                Gender = Gender.Male
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _unitOfWork.Commit();
            }
            CreateStudentDTO createStudentDTO = new CreateStudentDTO
            {
                StudentId = 5000,
                Name = "updated name",
                BirthDate = new DateTime(1998, 3, 26),
                Email = "updatedEmail@gmail.com",
                Address = "updated address",
                Gender = "Female",
                Version = student.Version,
                EditMode = true
            };

            // Act
            _studentService.UpdateStudent(createStudentDTO);

            // Assert
            Student updatedStudent;
            using (_unitOfWork.Start())
            {
                updatedStudent = _studentRepository.FindStudentByStudentId(student.StudentId);
            }
            _mockStudents.Add(updatedStudent);
            Assert.AreNotEqual(null, updatedStudent);
            AssertStudentWithDTO(createStudentDTO, updatedStudent);
        }

        [Test]
        public void UpdateStudent__UpdateNonExistStudent__ObjectNotExistsExceptionShouldBeThrown()
        {
            // Arrange
            int studentId;
            using (_unitOfWork.Start())
            {
                do
                {
                    studentId = new Random().Next(1000, 9999);
                }
                while (_studentRepository.FindStudentByStudentId(studentId) != null);
            }
            CreateStudentDTO createStudentDTO = new CreateStudentDTO
            {
                StudentId = studentId,
                Name = "name",
                Email = "emal",
                Address = "address",
                Gender = "Male",
                BirthDate = new DateTime(2000, 3, 23),
                EditMode = true
            };

            // Act
            void updateANonExistStudent() => _studentService.UpdateStudent(createStudentDTO);

            // Assert
            Assert.Throws(typeof(ObjectNotExistsException), updateANonExistStudent);
        }

        [Test]
        public void UpdateStudent__UpdateAStudentMissingName__MissingRequiredFieldExceptionShouldBeThrown()
        {
            // Arrange
            Student student = new Student
            {
                StudentId = 5000,
                Name = "Nguyễn Văn Đức",
                BirthDate = new DateTime(2000, 3, 26),
                Email = "email@gmail.com",
                Address = "mock address",
                Gender = Gender.Male
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _unitOfWork.Commit();
            }
            _mockStudents.Add(student);
            CreateStudentDTO createStudentDTO = new CreateStudentDTO
            {
                StudentId = 5000,
                BirthDate = new DateTime(1998, 3, 26),
                Email = "updatedEmail@gmail.com",
                Address = "updated address",
                Gender = "Female",
                Version = student.Version,
                EditMode = true
            };

            // Act
            void updateAMissingNameStudent() => _studentService.UpdateStudent(createStudentDTO);

            // Assert
            Assert.Throws(typeof(MissingRequiredFieldException), updateAMissingNameStudent);
        }

        [Test]
        public void UpdateStudent__UpdateAStudentWhichHasBeenUpdated__ObjectHasBeenUpdatedExceptionShouldBeThrown()
        {
            // Arrange
            Student student = new Student
            {
                StudentId = 5000,
                Name = "Nguyễn Văn Đức",
                BirthDate = new DateTime(2000, 3, 26),
                Email = "email@gmail.com",
                Address = "mock address",
                Gender = Gender.Male
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _unitOfWork.Commit();
            }
            _mockStudents.Add(student);
            CreateStudentDTO createStudentDTO = new CreateStudentDTO
            {
                StudentId = 5000,
                Name = "name",
                BirthDate = new DateTime(1998, 3, 26),
                Email = "updatedEmail@gmail.com",
                Address = "updated address",
                Gender = "Female",
                Version = student.Version,
                EditMode = true
            };
            using (_unitOfWork.Start())
            {
                student.Name = "new name";
                _genericRepository.Update(student);
                _unitOfWork.Commit();
            }

            // Act
            void updateAStudentWhichHasBeenUpdated() => _studentService.UpdateStudent(createStudentDTO);

            // Assert
            Assert.Throws(typeof(ObjectHasBeenUpdatedException), updateAStudentWhichHasBeenUpdated);
        }

        [Test]
        public void FindAllStudents__SaveThreeMockStudentsAndInvokeFindAllStudents__AllThreeMockStudentsShouldBeFound()
        {
            // Arrange
            Student student1 = new Student
            {
                StudentId = 2000,
                Name = "student 1",
                BirthDate = new DateTime(2000, 3, 26),
                Address = "address 1",
                Email = "email1@gmail.com",
                Gender = Gender.Male
            };
            Student student2 = new Student
            {
                StudentId = 2001,
                Name = "student 2",
                BirthDate = new DateTime(1999, 3, 26),
                Address = "address 2",
                Email = "email2@gmail.com",
                Gender = Gender.Female
            };
            Student student3 = new Student
            {
                StudentId = 2002,
                Name = "student 3",
                BirthDate = new DateTime(1998, 3, 26),
                Address = "address 3",
                Email = "email3@gmail.com",
                Gender = Gender.Male
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student1);
                _genericRepository.Save(student2);
                _genericRepository.Save(student3);
                _unitOfWork.Commit();
            }
            _mockStudents.Add(student1);
            _mockStudents.Add(student2);
            _mockStudents.Add(student3);

            // Act
            IList<IndexStudentDTO> indexStudentDTOs = _studentService.FindAllStudents();

            // Assert
            foreach (Student student in _mockStudents)
            {
                IndexStudentDTO indexStudentDTO = indexStudentDTOs.Where(x => x.StudentId == student.StudentId)
                    .FirstOrDefault();
                Assert.AreNotEqual(null, indexStudentDTO);
                AssertStudentWithDTO(indexStudentDTO, student);
            }
        }

        [Test]
        public void DeleteStudent__SaveOneMockStudentAndDeleteIt__TheStudentShouldBeDeletedSuccessfully()
        {
            // Arrange
            Student student = CreateOneMockStudent();
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _unitOfWork.Commit();
            }

            // Act
            _studentService.DeleteStudent(student.StudentId);

            // Assert
            Student foundStudent;
            using (_unitOfWork.Start())
            {
                foundStudent = _studentRepository.FindStudentByStudentId(student.StudentId);
            }
            Assert.AreEqual(null, foundStudent);
        }

        [Test]
        public void DeleteStudent__TryToDeleteANonExistStudent__ObjectNotExistsExceptionShouldBeThrown()
        {
            // Arrange
            int studentId;
            using (_unitOfWork.Start())
            {
                do
                {
                    studentId = new Random().Next(1000, 9999);
                }
                while (_studentRepository.FindStudentByStudentId(studentId) != null);
            }

            // Act
            void deleteANonExistStudent() =>_studentService.DeleteStudent(studentId);

            // Assert
            Assert.Throws(typeof(ObjectNotExistsException), deleteANonExistStudent);
        }

        [Test]
        public void IsStudentIdAlreadyExist__SaveOneMockStudentAndCheck__ShouldReturnTrue()
        {
            // Arrange
            Student student = CreateOneMockStudent();
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _unitOfWork.Commit();
            }
            _mockStudents.Add(student);

            // Act
            bool isStudentIdAlreadyExist = _studentService.IsStudentIdAlreadyExist(student.StudentId);

            // Assert
            Assert.AreEqual(true, isStudentIdAlreadyExist);
        }

        [Test]
        public void IsStudentIdAlreadyExist__CheckANonExistStudent__ShouldReturnFalse()
        {
            // Arrange
            int studentId;
            using (_unitOfWork.Start())
            {
                do
                {
                    studentId = new Random().Next(1000, 9999);
                }
                while (_studentRepository.FindStudentByStudentId(studentId) != null);
            }

            // Act
            bool isStudentIdAlreadyExist = _studentService.IsStudentIdAlreadyExist(studentId);

            // Assert
            Assert.AreEqual(false, isStudentIdAlreadyExist);
        }

        [Test]
        public void FindStudentByStudentId__SaveOneMockStudentAndFindIt__TheStudentShouldBeFound()
        {
            // Arrange
            Student student = CreateOneMockStudent();
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _unitOfWork.Commit();
            }
            _mockStudents.Add(student);

            // Act
            CreateStudentDTO createStudentDTO = _studentService.FindStudentByStudentId(student.StudentId);

            // Assert
            Assert.AreNotEqual(null, createStudentDTO);
            AssertStudentWithDTO(createStudentDTO, student);
        }

        private void AssertStudentWithDTO(CreateStudentDTO createStudentDTO, Student student)
        {
            Assert.AreEqual(createStudentDTO.StudentId, student.StudentId);
            Assert.AreEqual(createStudentDTO.Name, student.Name);
            Assert.AreEqual(createStudentDTO.BirthDate, student.BirthDate);
            Assert.AreEqual(createStudentDTO.Email, student.Email);
            Assert.AreEqual(createStudentDTO.Gender, student.Gender.ToString());
            Assert.AreEqual(createStudentDTO.Address, student.Address);
        }
        private void AssertStudentWithDTO(IndexStudentDTO indexStudentDTO, Student student)
        {
            Assert.AreEqual(indexStudentDTO.StudentId, student.StudentId);
            Assert.AreEqual(indexStudentDTO.Name, student.Name);
            Assert.AreEqual(indexStudentDTO.BirthDate, student.BirthDate);
            Assert.AreEqual(indexStudentDTO.Email, student.Email);
            Assert.AreEqual(indexStudentDTO.Gender, GenderHelper.GetText(student.Gender));
            Assert.AreEqual(indexStudentDTO.Address, student.Address);
        }

        private Student CreateOneMockStudent()
        {
            return new Student
            {
                StudentId = 3000,
                Name = "student 1",
                Email = "email 1",
                Gender = Gender.Male,
                Address = "address 1",
                BirthDate = new DateTime(2000, 3, 26)
            };
        }
    }
}

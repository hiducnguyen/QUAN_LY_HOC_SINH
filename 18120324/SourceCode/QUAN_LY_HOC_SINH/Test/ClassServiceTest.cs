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
    class ClassServiceTest
    {
        private IClassService _classService;
        private List<Class> _mockClasses;
        private List<Student> _mockStudents;
        private IUnitOfWork _unitOfWork;
        private IGenericRepository _genericRepository;
        private IClassRepository _classRepository;
        private IRuleRepository _ruleRepository;
        private IStudentRepository _studentRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mockClasses = new List<Class>();
            _mockStudents = new List<Student>();
            _unitOfWork = new UnitOfWork();
            _genericRepository = new GenericRepository(_unitOfWork);
            _classRepository = new ClassRepository(_unitOfWork);
            _studentRepository = new StudentRepository(_unitOfWork);
            _ruleRepository = new RuleRepository(_unitOfWork);
            _classService = new ClassService(_unitOfWork, _genericRepository,
                _classRepository, _studentRepository, _ruleRepository);
        }

        [TearDown]
        public void TearDown()
        {
            using (_unitOfWork.Start())
            {
                foreach (Class @class in _mockClasses)
                {
                    _genericRepository.Delete(@class);
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
        public void CreateClass__CreateOneValidClassWithoutStudent__TheClassShouldBeSavedSuccessfully()
        {
            // Arrange
            CreateClassDTO createClassDTO = new CreateClassDTO
            {
                Name = "test",
                Grade = 10
            };

            // Act
            Class savedClass = _classService.CreateClass(createClassDTO);

            // Assert
            _mockClasses.Add(savedClass);
            Assert.AreNotEqual(null, savedClass);
            AssertCreateClassDTOAndClass(createClassDTO, savedClass);
        }

        [Test]
        public void CreateClass__CreateOneValidClassWithOneStudent__TheClassShouldBeSavedAndTheStudentShouldBeUpdatedSuccessfully()
        {
            // Arrange
            Student student = new Student
            {
                StudentId = 5000,
                Name = "test",
                Address = "test",
                BirthDate = new DateTime(2000,3,26),
                Email = "test@gmail.com",
                Gender = Gender.Male,
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _unitOfWork.Commit();            
            }

            // Act
            CreateClassDTO createClassDTO = new CreateClassDTO
            {
                Name = "test",
                Grade = 11,
                Students = new List<int> { student.StudentId }
            };
            Class @class = _classService.CreateClass(createClassDTO);

            // Assert
            _mockClasses.Add(@class);
            Assert.AreNotEqual(null, @class);
            AssertCreateClassDTOAndClass(createClassDTO, @class);
            Student currentStudent;
            using (_unitOfWork.Start())
            {
                currentStudent = _studentRepository.FindStudentByStudentId(student.StudentId);
            }
            _mockStudents.Add(currentStudent);
            Assert.AreNotEqual(null, currentStudent);
            Assert.AreEqual(student.Name, currentStudent.Name);
            Assert.AreEqual(student.Gender, currentStudent.Gender);
            Assert.AreEqual(student.BirthDate, currentStudent.BirthDate);
            Assert.AreEqual(student.Address, currentStudent.Address);
            Assert.AreEqual(student.Email, currentStudent.Email);
            Assert.AreEqual(@class.Id, currentStudent.ClassId);
        }

        [Test]
        public void CreateClass__ClassNameAlreadyExist__ObjectAlreadyExistExceptionShouldBeThrown()
        {
            // Arrange
            Class @class = new Class { 
                Name = "test",
                Grade = 12
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(@class);
                _unitOfWork.Commit();
            }
            _mockClasses.Add(@class);
            CreateClassDTO createClassDTO = new CreateClassDTO
            {
                Name = @class.Name,
                Grade = 10
            };

            // Act
            void createClassWithClassNameAlreadyExist() => _classService.CreateClass(createClassDTO);

            // Assert
            Assert.Throws(typeof(ObjectAlreadyExistsException), createClassWithClassNameAlreadyExist);
        }

        [Test]
        public void CreateClass__MissingClassName__MissingRequiredFieldExceptionShouldBeThrown()
        {
            // Arrange
            CreateClassDTO createClassDTO = new CreateClassDTO
            {
                Grade = 12
            };

            // Act
            void createClassMissingClassName() => _classService.CreateClass(createClassDTO);

            // Assert
            Assert.Throws(typeof(MissingRequiredFieldException), createClassMissingClassName);
        }

        [Test]
        public void CreateClass__StudentAlreadyHaveClass__StudentAlreadyHaveClassExceptionShouldBeThrown()
        {
            // Arrange
            Student student = new Student
            {
                StudentId = 5000,
                Name = "test",
                Address = "test",
                Email = "test",
                BirthDate = new DateTime(2000,3,26),
                Gender = Gender.Male
            };
            Class @class = new Class
            {
                Name = "test",
                Grade = 10,
                Students = new HashSet<Student> { student}
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _genericRepository.Save(@class);
                _unitOfWork.Commit();
            }
            _mockStudents.Add(student);
            _mockClasses.Add(@class);
            CreateClassDTO createClassDTO = new CreateClassDTO
            {
                Name = "new test",
                Grade = 12,
                Students = new List<int> { student.StudentId }
            };

            // Act
            void createClassWithStudentAlreadyHaveClass() => _classService.CreateClass(createClassDTO);

            // Assert
            Assert.Throws(typeof(StudentAlreadyHaveClassException), createClassWithStudentAlreadyHaveClass);
        }

        [Test]
        public void CreateClass__StudentNotExist__ObjectNotExistExceptionShouldBeThrown()
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
            CreateClassDTO createClassDTO = new CreateClassDTO
            {
                Name = "test",
                Grade = 10,
                Students = new List<int> { studentId }
            };

            // Act
            void createClassWithStudentNotExist() => _classService.CreateClass(createClassDTO);

            // Assert
            Assert.Throws(typeof(ObjectNotExistsException), createClassWithStudentNotExist);
        }

        [Test]
        public void IsClassNameExist__ClassNameExist__TrueShouldBeReturned()
        {
            // Arrange
            Class @class = new Class
            {
                Name = "test",
                Grade = 10
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(@class);
                _unitOfWork.Commit();
            }
            _mockClasses.Add(@class);

            // Act
            bool isClassNameExist = _classService.IsClassNameExist(@class.Name);

            // Assert
            Assert.AreEqual(true, isClassNameExist);
        }

        [Test]
        public void IsClassNameExist__ClassNameNotExist__FalseShouldBeReturned()
        {
            // Arrange

            // Act
            bool isClassNameExist = _classService.IsClassNameExist("test");

            // Assert
            Assert.AreEqual(false, isClassNameExist);
        }

        [Test]
        public void FindClassByName__ClassNameExist__CreateClassDTOShouldBeReturnedSuccessfully()
        {
            // Arrange
            Student student = new Student
            {
                StudentId = 5000,
                Name = "test",
                Address = "test",
                Email = "test@gmail.com",
                Gender = Gender.Male,
                BirthDate = new DateTime(2000,3,26)
            };
            Class @class = new Class
            {
                Name = "test",
                Grade = 12,
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
            CreateClassDTO createClassDTO = _classService.FindClassByName(@class.Name);

            // Assert
            Assert.AreNotEqual(null, createClassDTO);
            Assert.AreEqual(@class.Name, createClassDTO.Name);
            Assert.AreEqual(@class.Grade, createClassDTO.Grade);
            Assert.AreEqual(@class.Students.Count, createClassDTO.Students.Count);
            Assert.AreEqual(student.StudentId, createClassDTO.Students.First());
        }

        [Test]
        public void FindClassByName__ClassNameNotExist__ObjectNotExistExceptionShouldBeThrown()
        {
            // Arrange

            // Act
            void findClassByClassNameNotExist() => _classService.FindClassByName("test");

            // Assert
            Assert.Throws(typeof(ObjectNotExistsException), findClassByClassNameNotExist);
        }

        [Test]
        public void UpdateClass__ValidClass__TheClassShouldBeUpdatedSuccessfully()
        {
            // Arrange
            Student student1 = new Student
            {
                StudentId = 5000,
                Name = "test one",
                Address = "test one",
                Email = "testone@gmail.com",
                Gender = Gender.Male,
                BirthDate = new DateTime(2000,3,26)
            };
            Student student2 = new Student
            {
                StudentId = 5001,
                Name = "test two",
                Address = "test two",
                Email = "testtwo@gmail.com",
                Gender = Gender.Female,
                BirthDate = new DateTime(1999, 3, 26)
            };
            Class @class = new Class
            {
                Name = "test",
                Grade = 10,
                Students = new HashSet<Student> { student1 }
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student1);
                _genericRepository.Save(student2);
                _genericRepository.Save(@class);
                _unitOfWork.Commit();
            }
            CreateClassDTO createClassDTO = new CreateClassDTO
            {
                Name = @class.Name,
                Grade = 12,
                Students = new List<int> { student1.StudentId },
                Version = @class.Version
            };
            createClassDTO.Students.Add(student2.StudentId);

            // Act
            _classService.UpdateClass(createClassDTO);

            // Assert
            Class updatedClass;
            using (_unitOfWork.Start())
            {
                updatedClass = _classRepository.FindClassByName(@class.Name);
            }
            _mockClasses.Add(updatedClass);
            foreach (Student student in updatedClass.Students)
            {
                _mockStudents.Add(student);
            }
            Assert.AreNotEqual(null, updatedClass);
            Assert.AreEqual(createClassDTO.Grade, updatedClass.Grade);
            Assert.AreNotEqual(null, updatedClass.Students);
            Assert.AreEqual(createClassDTO.Students.Count, updatedClass.Students.Count);
            foreach (Student student in updatedClass.Students)
            {
                Assert.IsTrue(createClassDTO.Students.Contains(student.StudentId));
            }
        }

        [Test]
        public void UpdateClass__ClassNotExist__ObjectNotExistExceptionShouldBeThrown()
        {
            // Arrange
            CreateClassDTO createClassDTO = new CreateClassDTO
            {
                Name = "test",
                Grade = 10
            };

            // Act
            void updateClassNotExist() => _classService.UpdateClass(createClassDTO);

            // Assert
            Assert.Throws(typeof(ObjectNotExistsException), updateClassNotExist);
        }

        [Test]
        public void UpdateClass__MissingGrade__MissingRequiredFieldExceptionShouldBeThrown()
        {
            // Arrange
            Class @class = new Class
            {
                Name = "test",
                Grade = 12
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(@class);
                _unitOfWork.Commit();
            }
            _mockClasses.Add(@class);
            CreateClassDTO createClassDTO = new CreateClassDTO
            {
                Name = @class.Name
            };

            // Act
            void updateClassMissingGrade() => _classService.UpdateClass(createClassDTO);

            // Assert
            Assert.Throws(typeof(MissingRequiredFieldException), updateClassMissingGrade);
        }

        [Test]
        public void UpdateClass__ClassHasBeenUpdated__ObjectHasBeenUpdatedExceptionShouldBeThrown()
        {
            // Arrange
            Class @class = new Class
            {
                Name = "test",
                Grade = 10
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(@class);
                _unitOfWork.Commit();
            }
            _mockClasses.Add(@class);
            CreateClassDTO createClassDTO = new CreateClassDTO
            {
                Name = @class.Name,
                Grade = 12,
                Version = @class.Version
            };
            @class.Grade = 11;
            using (_unitOfWork.Start())
            {
                _genericRepository.Update(@class);
                _unitOfWork.Commit();
            }

            // Act
            void updateClassHasBeenUpdated() => _classService.UpdateClass(createClassDTO);

            // Assert
            Assert.Throws(typeof(ObjectHasBeenUpdatedException), updateClassHasBeenUpdated);
        }

        [Test]
        public void UpdateClass__StudentAlreadyHaveClass__StudentAlreadyHaveClassExceptionShouldBeThrown()
        {
            // Arrange
            Student student = new Student
            {
                StudentId = 5000,
                Name = "test",
                Address = "test",
                BirthDate = new DateTime(2000,3,26),
                Email = "test@gmail.com",
                Gender = Gender.Female
            };
            Class class1 = new Class
            {
                Name = "test one",
                Grade = 10,
                Students = new HashSet<Student> { student }
            };
            Class class2 = new Class
            {
                Name = "test two",
                Grade = 11
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _genericRepository.Save(class1);
                _genericRepository.Save(class2);
                _unitOfWork.Commit();
            }
            _mockStudents.Add(student);
            _mockClasses.Add(class1);
            _mockClasses.Add(class2);

            // Act
            CreateClassDTO createClassDTO = new CreateClassDTO
            {
                Name = class2.Name,
                Grade = class2.Grade,
                Version = class2.Version,
                Students = new List<int> { student.StudentId }
            };
            void updateClassWithStudentAlreadyHaveClass() => _classService.UpdateClass(createClassDTO);

            // Assert
            Assert.Throws(typeof(StudentAlreadyHaveClassException), updateClassWithStudentAlreadyHaveClass);
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
            IList<IndexClassDTO> classes = _classService.FindAllClasses();

            // Assert
            foreach (Class @class in _mockClasses)
            {
                IndexClassDTO foundClassDTO = classes.Where(x => x.Name == @class.Name).FirstOrDefault();
                Assert.AreNotEqual(null, foundClassDTO);
                Assert.AreEqual(@class.Grade, foundClassDTO.Grade);
                if (@class.Students == null)
                {
                    Assert.AreEqual(0, foundClassDTO.NumberOfStudents);
                }
                else
                {
                    Assert.AreEqual(@class.Students.Count, foundClassDTO.NumberOfStudents);
                }
            }
        }

        [Test]
        public void DeleteClass__SaveOneMockClassAndDeleteIt__TheClassShouldBeDeletedSuccessfully()
        {
            // Arrange
            Student student = new Student
            {
                StudentId = 5000,
                Name = "test",
                Address = "test",
                Email = "test@gmail.com",
                Gender = Gender.Male,
                BirthDate = new DateTime(2000, 3, 26)
            };
            Class @class = new Class
            {
                Name = "test",
                Grade = 11,
                Students = new HashSet<Student> { student }
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(student);
                _genericRepository.Save(@class);
                _unitOfWork.Commit();
            }

            // Act
            _classService.DeleteClass(@class.Name);

            // Assert
            using (_unitOfWork.Start())
            {
                _mockStudents.Add(_studentRepository.FindStudentByStudentId(student.StudentId));
                Class foundClass = _classRepository.FindClassByName(@class.Name);
                Assert.AreEqual(null, foundClass);
            }
        }

        [Test]
        public void DeleteClass__DeleteNotExistClass__ObjectNotExistExceptioNShouldBeThrown()
        {
            // Arrange
            string className = "test";

            // Act
            void deleteClassNotExist() => _classService.DeleteClass(className);

            // Assert
            Assert.Throws(typeof(ObjectNotExistsException), deleteClassNotExist);
        }

        private void AssertCreateClassDTOAndClass(CreateClassDTO createClassDTO, Class @class)
        {
            Assert.AreEqual(createClassDTO.Name, @class.Name);
            Assert.AreEqual(createClassDTO.Grade, @class.Grade);
            IList<int> studentIds = GetStudentIdsFromStudents(@class.Students);
            if (createClassDTO.Students != null || studentIds != null)
            {
                Assert.AreNotEqual(null, createClassDTO.Students);
                Assert.AreNotEqual(null, studentIds);
                Assert.AreEqual(createClassDTO.Students.Count, studentIds.Count);
                foreach (int studentId in createClassDTO.Students)
                {
                    Assert.IsTrue(studentIds.Contains(studentId));
                }
            }
        }
        private IList<int> GetStudentIdsFromStudents(ISet<Student> students)
        {
            if (students != null)
            {
                IList<int> studentIds = new List<int>();
                foreach (Student student in students)
                {
                    studentIds.Add(student.StudentId);
                }
                return studentIds;
            }
            else
            {
                return null;
            }
        }
    }
}

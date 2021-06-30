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
    class SubjectServiceTest
    {
        private ISubjectService _subjectService;
        private IUnitOfWork _unitOfWork;
        private ISubjectRepository _subjectRepository;
        private IGenericRepository _genericRepository;
        private ITranscriptRepository _transcriptRepository; 
        private IList<Subject> _mockSubjects;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _unitOfWork = new UnitOfWork();
            _genericRepository = new GenericRepository(_unitOfWork);
            _subjectRepository = new SubjectRepository(_unitOfWork);
            _transcriptRepository = new TranscriptRepository(_unitOfWork);
            _subjectService = new SubjectService(_unitOfWork, _subjectRepository,
                _genericRepository, _transcriptRepository);
            _mockSubjects = new List<Subject>();
        }

        [TearDown]
        public void TearDown()
        {
            using (_unitOfWork.Start())
            {
                foreach (Subject subject in _mockSubjects)
                {
                    _genericRepository.Delete(subject);
                }
                _unitOfWork.Commit();
            }
            _mockSubjects.Clear();
        }

        [Test]
        public void FindAllSubjects__SaveTwoMockSubjects__AllTwoSubjectsShouldBeFoundSuccessfully()
        {
            // Arrange
            Subject subject1 = new Subject
            {
                SubjectId = 500,
                Name = "test one"
            };
            Subject subject2 = new Subject
            {
                SubjectId = 501,
                Name = "test two"
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(subject1);
                _genericRepository.Save(subject2);
                _unitOfWork.Commit();
            }
            _mockSubjects.Add(subject1);
            _mockSubjects.Add(subject2);

            // Act
            IList<IndexSubjectDTO> indexSubjectDTOs = _subjectService.FindAllSubjects();

            // Assert
            foreach (Subject subject in _mockSubjects)
            {
                IndexSubjectDTO indexSubjectDTO = indexSubjectDTOs.Where(x => x.SubjectId == subject.SubjectId)
                                                                  .FirstOrDefault();
                Assert.AreNotEqual(null, indexSubjectDTO);
                Assert.AreEqual(subject.Name, indexSubjectDTO.Name);
            }
        }

        [Test]
        public void CreateSubject__SaveOneValidSubject__TheSubjectShouldBeCreatedSuccessfully()
        {
            // Arrange
            CreateSubjectDTO createSubjectDTO = new CreateSubjectDTO
            {
                SubjectId = 500,
                Name = "test"
            };

            // Act
            Subject subject = _subjectService.CreateSubject(createSubjectDTO);

            // Assert
            _mockSubjects.Add(subject);
            Assert.AreNotEqual(null, subject);
            Assert.AreEqual(createSubjectDTO.SubjectId, subject.SubjectId);
            Assert.AreEqual(createSubjectDTO.Name, subject.Name);
        }

        [Test]
        public void CreateSubject__SaveSubjectMisingName__MissingRequiredFieldExceptionShouldBeThrown()
        {
            // Arrange
            CreateSubjectDTO createSubjectDTO = new CreateSubjectDTO
            {
                SubjectId = 500
            };

            // Act
            void createSubjectMissingName() => _subjectService.CreateSubject(createSubjectDTO);

            // Assert
            Assert.Throws(typeof(MissingRequiredFieldException), createSubjectMissingName);
        }

        [Test]
        public void CreateSubject__SaveSubjectWithNameAlreadyExist__ObjectAlreadyExistExceptionShouldBeThrown()
        {
            // Arrange
            Subject subject = new Subject
            {
                Name = "test",
                SubjectId = 500
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(subject);
                _unitOfWork.Commit();
            }
            _mockSubjects.Add(subject);
            CreateSubjectDTO createSubjectDTO = new CreateSubjectDTO
            {
                Name = subject.Name,
                SubjectId = 501
            };

            // Act
            void createSubjectWithNameAlreadyExist() => _subjectService.CreateSubject(createSubjectDTO);

            // Assert
            Assert.Throws(typeof(ObjectAlreadyExistsException), createSubjectWithNameAlreadyExist);
        }

        [Test]
        public void IsSubjectIdAlreadyExist__SaveOneMockSubjectAndCheckByItsId__TrueShouldBeReturned()
        {
            // Arrange
            Subject subject = new Subject
            {
                Name = "test",
                SubjectId = 500
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(subject);
                _unitOfWork.Commit();
            }
            _mockSubjects.Add(subject);

            // Act
            bool isSubjectIdAlreadyExist = _subjectService.IsSubjectIdAlreadyExist(subject.SubjectId);

            // Assert
            Assert.AreEqual(true, isSubjectIdAlreadyExist);
        }

        [Test]
        public void IsSubjectIdAlreadyExist__UseNotExistSubjectIdToCheck__FalseShouldBeReturned()
        {
            // Arrange
            int subjectId;
            using (_unitOfWork.Start())
            {
                do
                {
                    subjectId = new Random().Next(100, 999);
                }
                while (_subjectRepository.FindSubjectBySubjectId(subjectId) != null);
            }

            // Act
            bool isSubjectIdAlreadyExist = _subjectService.IsSubjectIdAlreadyExist(subjectId);

            // Assert
            Assert.AreEqual(false, isSubjectIdAlreadyExist);
        }

        [Test]
        public void IsSubjectNameAlreadyExist__SaveOneMockSubjectAndCheckByItsName__TrueShouldBeReturned()
        {
            // Arrange
            Subject subject = new Subject
            {
                Name = "test",
                SubjectId = 500
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(subject);
                _unitOfWork.Commit();
            }
            _mockSubjects.Add(subject);

            // Act
            bool isSubjectNameAlreadyExist = _subjectService.IsSubjectNameAlreadyExist(subject.Name);

            // Assert
            Assert.AreEqual(true, isSubjectNameAlreadyExist);
        }

        [Test]
        public void IsSubjectNameAlreadyExist__UseNotExistSubjectNameToCheck__FalseShouldBeReturned()
        {
            // Arrange
            string name = "test";

            // Act
            bool isSubjectIdAlreadyExist = _subjectService.IsSubjectNameAlreadyExist(name);

            // Assert
            Assert.AreEqual(false, isSubjectIdAlreadyExist);
        }

        [Test]
        public void DeleteSubject__SaveOneMockSubjectAndDeleteIt__TheSubjectShouldBeDeletedSuccessfully()
        {
            // Arrange
            Subject subject = new Subject
            {
                Name = "test",
                SubjectId = 500
            };
            Student student = new Student
            {
                StudentId =  3000,
                Name = "test",
                Address = "test",
                BirthDate = new DateTime(2000,3,26),
                Email = "test@gmail.com",
                Gender = Gender.Female
            };
            Transcript transcript = new Transcript
            {
                FifteenMinuteTestScore = 5,
                FortyFiveMinuteTestScore = 5,
                FinalTestScore = 5,
                Semester = Semester.First,
                Subject = subject
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(subject);
                _genericRepository.Save(student);
                transcript.StudentId = student.Id;
                _genericRepository.Save(transcript);
                _unitOfWork.Commit();
            }

            // Act
            _subjectService.DeleteSubject(subject.SubjectId);

            // Assert
            using (_unitOfWork.Start())
            {
                Subject foundSubject = _subjectRepository.FindSubjectBySubjectId(subject.SubjectId);
                IList<Transcript> foundTranscripts = _transcriptRepository.FindAllTranscripts(subject);
                Assert.AreEqual(null, foundSubject);
                Assert.AreEqual(0, foundTranscripts.Count);
            }

            // Tear down
            using (_unitOfWork.Start())
            {
                _genericRepository.Delete(student);
                _unitOfWork.Commit();
            }
        }

        [Test]
        public void DeleteSubject__DeleteByNotExistSubjectId__ObjectNotExistExceptionShouldBeThrown()
        {
            // Arrange
            int subjectId;
            using (_unitOfWork.Start())
            {
                do
                {
                    subjectId = new Random().Next(100, 999);
                }
                while (_subjectRepository.FindSubjectBySubjectId(subjectId) != null);
            }

            // Act
            void deleteByNotExistSubjectId() => _subjectService.DeleteSubject(subjectId);

            // Assert
            Assert.Throws(typeof(ObjectNotExistsException), deleteByNotExistSubjectId);
        }

        [Test]
        public void FindSubjectBySubjectId__SaveOneMockSubjectAndFindIt__TheSubjectDTOShouldBeFoundSuccessfully()
        {
            // Arrange
            Subject subject = new Subject
            {
                Name = "test",
                SubjectId = 500
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(subject);
                _unitOfWork.Commit();
            }
            _mockSubjects.Add(subject);

            // Act
            CreateSubjectDTO createSubjectDTO = _subjectService.FindSubjectBySubjectId(subject.SubjectId);

            // Assert
            Assert.AreNotEqual(null, createSubjectDTO);
            Assert.AreEqual(subject.Name, createSubjectDTO.Name);
            Assert.AreEqual(subject.SubjectId, createSubjectDTO.SubjectId);
            Assert.AreEqual(subject.Version, createSubjectDTO.Version);
        }

        [Test]
        public void UpdateSubject__UpdateOneValidSubject__TheSubjectShouldBeUpdatedSuccessfully()
        {
            // Arrange
            Subject subject = new Subject
            {
                Name = "test",
                SubjectId = 500
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(subject);
                _unitOfWork.Commit();
            }
            CreateSubjectDTO createSubjectDTO = new CreateSubjectDTO
            {
                Name = "new name",
                SubjectId = subject.SubjectId,
                Version = subject.Version
            };

            // Act
            _subjectService.UpdateSubject(createSubjectDTO);

            // Assert
            Subject foundSubject;
            using (_unitOfWork.Start())
            {
                foundSubject = _subjectRepository.FindSubjectBySubjectId(subject.SubjectId);
            }
            _mockSubjects.Add(foundSubject);
            Assert.AreNotEqual(null, foundSubject);
            Assert.AreEqual(createSubjectDTO.Name, foundSubject.Name);
        }

        [Test]
        public void UpdateSubject__UpdateSubjectMissingName__MissingRequiredFieldExceptionShouldBeThrown()
        {
            // Arrange
            Subject subject = new Subject
            {
                Name = "test",
                SubjectId = 500
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(subject);
                _unitOfWork.Commit();
            }
            _mockSubjects.Add(subject);
            CreateSubjectDTO createSubjectDTO = new CreateSubjectDTO
            {
                SubjectId = subject.SubjectId,
                Version = subject.Version
            };

            // Act
            void updateSubjectWithoutName() => _subjectService.UpdateSubject(createSubjectDTO);

            // Assert
            Assert.Throws(typeof(MissingRequiredFieldException), updateSubjectWithoutName);
        }

        [Test]
        public void UpdateSubject__UpdateSubjectNotExist__ObjectNotExistsExceptionShouldBeThrown()
        {
            // Arrange
            CreateSubjectDTO createSubjectDTO = new CreateSubjectDTO
            {
                Name = "test",
                SubjectId = 500
            };

            // Act
            void updateSubjectNotExist() => _subjectService.UpdateSubject(createSubjectDTO);

            // Assert
            Assert.Throws(typeof(ObjectNotExistsException), updateSubjectNotExist);
        }

        [Test]
        public void UpdateSubject__UpdateSubjectHasBeenUpdated__ObjectHasBeenUpdatedExceptionShouldBeThrown()
        {
            // Arrange
            Subject subject = new Subject
            {
                Name = "test",
                SubjectId = 500
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(subject);
                _unitOfWork.Commit();
            }
            CreateSubjectDTO createSubjectDTO = new CreateSubjectDTO
            {
                Name = "new name",
                SubjectId = subject.SubjectId,
                Version = subject.Version
            };
            subject.Name = "updated";
            using (_unitOfWork.Start())
            {
                _genericRepository.Update(subject);
                _unitOfWork.Commit();
            }
            _mockSubjects.Add(subject);

            // Act
            void updateSubjectHasBeenUpdated() => _subjectService.UpdateSubject(createSubjectDTO);

            // Assert
            Assert.Throws(typeof(ObjectHasBeenUpdatedException), updateSubjectHasBeenUpdated);
        }

        [Test]
        public void UpdateSubject__UpdateSubjectWithNameAlreadyTaken__ObjectAlreadyExistExceptionShouldBeThrown()
        {
            // Arrange
            Subject subject1 = new Subject
            {
                Name = "test",
                SubjectId = 500
            };
            Subject subject2 = new Subject
            {
                Name = "test two",
                SubjectId = 501
            };
            using (_unitOfWork.Start())
            {
                _genericRepository.Save(subject1);
                _genericRepository.Save(subject2);
                _unitOfWork.Commit();
            }
            _mockSubjects.Add(subject1);
            _mockSubjects.Add(subject2);
            CreateSubjectDTO createSubjectDTO = new CreateSubjectDTO
            {
                Name = subject2.Name,
                SubjectId = subject1.SubjectId,
                Version = subject1.Version
            };

            // Act
            void updateWithNameAlreadyTaken() => _subjectService.UpdateSubject(createSubjectDTO);

            // Assert
            Assert.Throws(typeof(ObjectAlreadyExistsException), updateWithNameAlreadyTaken);
        }
    }
}

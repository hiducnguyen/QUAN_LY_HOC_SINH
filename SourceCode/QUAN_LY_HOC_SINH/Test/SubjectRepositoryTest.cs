using NUnit.Framework;
using Repositories;
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
    public class SubjectRepositoryTest
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository _genericRepository;
        private ISubjectRepository _subjectRepository;
        private IList<Subject> _mockSubjects;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _unitOfWork = new UnitOfWork();
            _genericRepository = new GenericRepository(_unitOfWork);
            _subjectRepository = new SubjectRepository(_unitOfWork);
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
            IList<Subject> subjects;
            using (_unitOfWork.Start())
            {
                subjects = _subjectRepository.FindAllSubjects();
            }

            // Assert
            foreach (Subject subject in _mockSubjects)
            {
                Subject foundSubject = subjects.Where(x => x.SubjectId == subject.SubjectId).FirstOrDefault();
                Assert.AreNotEqual(null, foundSubject);
                Assert.AreEqual(subject.Name, foundSubject.Name);
            }
        }

        [Test]
        public void FindSubjectBySubjectId__SaveOneMockSubjectAndFindById__TheSubjectShouldBeFoundSuccessfully()
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
            Subject foundSubject;
            using (_unitOfWork.Start())
            {
                foundSubject = _subjectRepository.FindSubjectBySubjectId(subject.SubjectId);
            }

            // Assert
            Assert.AreNotEqual(null, foundSubject);
            Assert.AreEqual(subject.Name, foundSubject.Name);
            Assert.AreEqual(subject.SubjectId, foundSubject.SubjectId);
        }

        [Test]
        public void FindSubjectByName__SaveOneMockSubjectAndFindByName__TheSubjectShouldBeFoundSuccessfully()
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
            Subject foundSubject;
            using (_unitOfWork.Start())
            {
                foundSubject = _subjectRepository.FindSubjectByName(subject.Name);
            }

            // Assert
            Assert.AreNotEqual(null, foundSubject);
            Assert.AreEqual(subject.Name, foundSubject.Name);
            Assert.AreEqual(subject.SubjectId, foundSubject.SubjectId);
        }
    }
}

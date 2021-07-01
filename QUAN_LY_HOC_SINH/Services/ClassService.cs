using NHibernate;
using Repositories;
using Repositories.Models;
using Repositories.UnitOfWork;
using Resources;
using Services.DTO;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services
{
    public class ClassService : IClassService
    {

        private IGenericRepository _genericRepository;
        private IClassRepository _classRepository;
        private IStudentRepository _studentRepository;
        private IRuleRepository _ruleRepository;
        private IUnitOfWork _unitOfWork;

        private bool IsMissingRequiredField(CreateClassDTO createClassDTO)
        {
            return (createClassDTO.Name == default) || (createClassDTO.Grade == default);
        }

        public ClassService(
            IUnitOfWork unitOfWork,
            IGenericRepository genericRepository,
            IClassRepository classRepository,
            IStudentRepository studentRepository,
            IRuleRepository ruleRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
            _classRepository = classRepository;
            _studentRepository = studentRepository;
            _ruleRepository = ruleRepository;
        }

        public Class CreateClass(CreateClassDTO createClassDTO)
        {
            if (IsMissingRequiredField(createClassDTO))
            {
                throw new MissingRequiredFieldException();
            }
            if (createClassDTO.Students != null)
            {
                using (_unitOfWork.Start())
                {
                    int maximumNumberOfStudentsInOneClass =
                        Convert.ToInt32(_ruleRepository.FindRuleById(1000).Value);
                    if (createClassDTO.Students.Count > maximumNumberOfStudentsInOneClass)
                        throw new OutOfMaximumNumberOfStudentsInClassException(maximumNumberOfStudentsInOneClass);
                }
            }
            Class @class = new Class
            {
                Name = createClassDTO.Name,
                Grade = createClassDTO.Grade
            };
            using (_unitOfWork.Start())
            {
                if (_classRepository.FindClassByName(createClassDTO.Name) != null)
                {
                    throw new ObjectAlreadyExistsException(Resource.Class, Resource.Name, createClassDTO.Name);
                }
                if (createClassDTO.Students != null)
                {
                    @class.Students = new HashSet<Student>();

                    foreach (int studentId in createClassDTO.Students)
                    {
                        Student student = _studentRepository.FindStudentByStudentId(studentId);
                        if (student == null)
                        {
                            throw new ObjectNotExistsException(Resource.Student, Resource.StudentId, studentId);
                        }
                        if (student.ClassId != null)
                        {
                            throw new StudentAlreadyHaveClassException(studentId);
                        }
                        @class.Students.Add(student);
                    }
                }
                _genericRepository.Save(@class);
                _unitOfWork.Commit();
            }
            return @class;
        }

        public SelectList GetAllGrades()
        {
            IEnumerable<SelectListItem> allGrades = new List<SelectListItem>(Enumerable.Range(10, 3)
                .Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = x.ToString()
                })
            );

            return new SelectList(allGrades.OrderBy(x => x.Text), "Value", "Text");
        }

        public bool IsClassNameExist(string name)
        {
            using (_unitOfWork.Start())
            {
                Class @class = _classRepository.FindClassByName(name);
                return @class != null;
            }
        }

        public CreateClassDTO FindClassByName(string name)
        {
            Class @class;
            using (_unitOfWork.Start())
            {
                @class = _classRepository.FindClassByName(name);
            }

            if (@class == null)
            {
                throw new ObjectNotExistsException(Resource.Class, Resource.Name, name);
            }
            CreateClassDTO createClassDTO = new CreateClassDTO
            {
                Name = @class.Name,
                Grade = @class.Grade,
                Version = @class.Version
            };
            if (@class.Students != null)
            {
                createClassDTO.Students = new List<int>();
                foreach (Student student in @class.Students)
                {
                    createClassDTO.Students.Add(student.StudentId);
                }
            }
            return createClassDTO;
        }

        public void UpdateClass(CreateClassDTO createClassDTO)
        {
            if (IsMissingRequiredField(createClassDTO))
            {
                throw new MissingRequiredFieldException();
            }
            if (createClassDTO.Students != null)
            {
                using (_unitOfWork.Start())
                {
                    int maximumNumberOfStudentsInOneClass =
                        Convert.ToInt32(_ruleRepository.FindRuleById(1000).Value);
                    if (createClassDTO.Students.Count > maximumNumberOfStudentsInOneClass)
                        throw new OutOfMaximumNumberOfStudentsInClassException(maximumNumberOfStudentsInOneClass);
                }
            }
            Class @class;
            using (_unitOfWork.Start())
            {
                @class = _classRepository.FindClassByName(createClassDTO.Name);
            }
            if (@class == null)
            {
                throw new ObjectNotExistsException(Resource.Class, Resource.Name, createClassDTO.Name);
            }
            if (@class.Version != createClassDTO.Version)
            {
                throw new ObjectHasBeenUpdatedException(Resource.Class, Resource.Name, createClassDTO.Name);
            }

            @class.Students = null;
            if (createClassDTO.Students != null)
            {
                @class.Students = new HashSet<Student>();
                using (_unitOfWork.Start())
                {
                    foreach (int studentId in createClassDTO.Students)
                    {
                        Student student = _studentRepository.FindStudentByStudentId(studentId);
                        if (student.ClassId != null && student.ClassId != @class.Id)
                        {
                            throw new StudentAlreadyHaveClassException(studentId);
                        }
                        @class.Students.Add(student);
                    }
                }
            }

            @class.Name = createClassDTO.Name;
            @class.Grade = createClassDTO.Grade;
            using (_unitOfWork.Start())
            {
                _genericRepository.Update(@class);
                _unitOfWork.Commit();
            }
        }

        IList<IndexClassDTO> IClassService.FindAllClasses()
        {
            IList<Class> classes;
            using (_unitOfWork.Start())
            {
                classes = _classRepository.FindAllClasses();
                foreach (Class @class in classes)
                {
                    NHibernateUtil.Initialize(@class.Students);
                }
            }
            if (classes == null) return null;
            IList<IndexClassDTO> indexClassDTOs = new List<IndexClassDTO>();
            foreach (Class @class in classes)
            {
                IndexClassDTO indexClassDTO = new IndexClassDTO
                {
                    Name = @class.Name,
                    Grade = @class.Grade
                };
                indexClassDTO.NumberOfStudents = (@class.Students == null) ? 0 : @class.Students.Count;
                indexClassDTOs.Add(indexClassDTO);
            }
            return indexClassDTOs;
        }

        public void DeleteClass(string className)
        {
            using (_unitOfWork.Start())
            {
                Class @class = _classRepository.FindClassByName(className);

                if (@class == null) throw new ObjectNotExistsException(Resource.Class, Resource.Name, className);
                
                _genericRepository.Delete(@class);
                _unitOfWork.Commit();
            }
        }

        public SelectList GetSelectListClasses()
        {
            IEnumerable<SelectListItem> allClasses;
            using (_unitOfWork.Start())
            {
                allClasses = new List<SelectListItem>(
                    _classRepository.FindAllClasses().Select(x => new SelectListItem
                    {
                        Value = x.Name.ToString(),
                        Text = x.Name.ToString()
                    })
                );
            }

            return new SelectList(allClasses.OrderBy(x => x.Text), "Value", "Text");
        }
    }
}

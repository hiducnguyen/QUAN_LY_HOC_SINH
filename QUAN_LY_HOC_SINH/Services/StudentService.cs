using Repositories;
using Repositories.Enums;
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
    public class StudentService : IStudentService
    {
        private IGenericRepository _genericRepository;
        private IStudentRepository _studentRepository;
        private IClassRepository _classRepository;
        private ISubjectRepository _subjectRepository;
        private IUnitOfWork _unitOfWork;

        private Student MapDTOtoStudent(CreateStudentDTO createStudentDTO, bool editMode = false)
        {
            return new Student
            {
                StudentId = createStudentDTO.StudentId,
                Name = createStudentDTO.Name,
                BirthDate = createStudentDTO.BirthDate,
                Email = createStudentDTO.Email,
                Address = createStudentDTO.Address,
                Gender = GenderHelper.ToGender(createStudentDTO.Gender),
                Version = (editMode) ? createStudentDTO.Version : default
            };
        }
        private IndexStudentDTO MapStudentToIndexStudentDTO(Student student)
        {
            return new IndexStudentDTO
            {
                StudentId = student.StudentId,
                Name = student.Name,
                BirthDate = student.BirthDate,
                Email = student.Email,
                Address = student.Address,
                Gender = GenderHelper.GetText(student.Gender)
            };
        }
        private CreateStudentDTO MapStudentToCreateStudentDTO(Student student)
        {
            return new CreateStudentDTO
            {
                StudentId = student.StudentId,
                Name = student.Name,
                BirthDate = student.BirthDate,
                Email = student.Email,
                Address = student.Address,
                Gender = student.Gender.ToString(),
                Version = student.Version
            };
        }
        private bool IsMissingRequiredField(CreateStudentDTO createStudentDTO)
        {
            return (createStudentDTO.StudentId == default) || (createStudentDTO.Name == default) ||
                (createStudentDTO.Address == default) || (createStudentDTO.BirthDate == default) ||
                (createStudentDTO.Gender == default) || (createStudentDTO.Email == default);
        }

        public StudentService(
            IUnitOfWork unitOfWork,
            IGenericRepository genericRepository,
            IStudentRepository studentRepository,
            IClassRepository classRepository,
            ISubjectRepository subjectRepository)
        {
            _genericRepository = genericRepository;
            _studentRepository = studentRepository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _unitOfWork = unitOfWork;
        }
        public Student CreateStudent(CreateStudentDTO createStudentDTO)
        {
            if (IsMissingRequiredField(createStudentDTO))
            {
                throw new MissingRequiredFieldException();
            }

            Student student = MapDTOtoStudent(createStudentDTO);

            using (_unitOfWork.Start())
            {
                if (_studentRepository.FindStudentByStudentId(student.StudentId) != null)
                {
                    throw new ObjectAlreadyExistsException(Resource.Student, Resource.StudentId, student.StudentId);
                }
                _genericRepository.Save(student);

                IList<Subject> subjects = _subjectRepository.FindAllSubjects();
                IList<Semester> semesters = SemesterHelper.GetAllSemesters();

                foreach (Subject subject in subjects)
                {
                    foreach (Semester semester in semesters)
                    {
                        Transcript transcript = new Transcript
                        {
                            Semester = semester,
                            StudentId = student.Id,
                            Subject = subject
                        };
                        _genericRepository.Save(transcript);
                    }
                }
                _unitOfWork.Commit();
            }

            return student;
        }

        public void DeleteStudent(int studentId)
        {
            using (_unitOfWork.Start())
            {
                Student student = _studentRepository.FindStudentByStudentId(studentId);
                if (student == null)
                {
                    throw new ObjectNotExistsException(Resource.Student, Resource.StudentId, studentId);
                }
                foreach (Transcript transcript in student.Transcripts)
                {
                    _genericRepository.Delete(transcript);
                }
                _unitOfWork.Commit();
            }
            using (_unitOfWork.Start())
            {
                Student student = _studentRepository.FindStudentByStudentId(studentId);
                _genericRepository.Delete(student);
                _unitOfWork.Commit();
            }
        }

        public IList<IndexStudentDTO> FindAllStudents()
        {
            IList<Student> students;
            using (_unitOfWork.Start())
            {
                students = _studentRepository.FindAllStudents();
            }

            IList<IndexStudentDTO> indexStudentDTOs = new List<IndexStudentDTO>();
            foreach (Student student in students)
            {
                indexStudentDTOs.Add(MapStudentToIndexStudentDTO(student));
            }

            return indexStudentDTOs;

        }

        public CreateStudentDTO FindStudentByStudentId(int studentId)
        {
            Student student;
            using (_unitOfWork.Start())
            {
                student = _studentRepository.FindStudentByStudentId(studentId);
            }
            if (student == null) throw new ObjectNotExistsException(Resource.Student, Resource.StudentId, studentId);
            CreateStudentDTO createStudentDTO = MapStudentToCreateStudentDTO(student);
            return createStudentDTO;
        }

        public bool IsStudentIdAlreadyExist(int studentId)
        {
            using (_unitOfWork.Start())
            {
                Student student = _studentRepository.FindStudentByStudentId(studentId);
                return (student != null);
            }
        }

        public void UpdateStudent(CreateStudentDTO createStudentDTO)
        {
            if (IsMissingRequiredField(createStudentDTO))
            {
                throw new MissingRequiredFieldException();
            }

            Student student = MapDTOtoStudent(createStudentDTO, true);
            using (_unitOfWork.Start())
            {
                Student currentStudent = _studentRepository.FindStudentByStudentId(student.StudentId);
                if (currentStudent == null)
                {
                    throw new ObjectNotExistsException(Resource.Student, Resource.StudentId, student.StudentId);
                }
                if (currentStudent.Version != student.Version)
                {
                    throw new ObjectHasBeenUpdatedException(Resource.Student, Resource.StudentId, student.StudentId);
                }

                currentStudent.Name = student.Name;
                currentStudent.Gender = student.Gender;
                currentStudent.Email = student.Email;
                currentStudent.Address = student.Address;
                currentStudent.BirthDate = student.BirthDate;

                _genericRepository.Update(currentStudent);
                _unitOfWork.Commit();
            }
        }

        public MultiSelectList GetAllAvailableStudents()
        {
            IEnumerable<SelectListItem> availableStudents;
            using (_unitOfWork.Start())
            {
                availableStudents = new List<SelectListItem>(
                    _studentRepository.FindAllAvailableStudents().Select(x => new SelectListItem
                    {
                        Value = x.StudentId.ToString(),
                        Text = $"{x.StudentId}: {x.Name}"
                    })
                );
            }

            return new MultiSelectList(availableStudents.OrderBy(x => x.Text), "Value", "Text");
        }

        public MultiSelectList GetAllAvailableStudents(string className)
        {
            IList<Student> students;
            using (_unitOfWork.Start())
            {
                Class @class = _classRepository.FindClassByName(className);
                if (@class == null)
                {
                    throw new ObjectNotExistsException(Resource.Class, Resource.Name, className);
                }
                IList<Student> currentStudentsOfClass = _studentRepository.FindStudentsByClassId(@class.Id);
                IList<Student> studentsWithNoClass = _studentRepository.FindAllAvailableStudents();
                students = currentStudentsOfClass.Concat(studentsWithNoClass).ToList();
            }
            IEnumerable <SelectListItem> selectListStudents = new List<SelectListItem>(
                students.Select(x => new SelectListItem
                {
                    Value = x.StudentId.ToString(),
                    Text = $"{x.StudentId}: {x.Name}"
                })
            );
            return new MultiSelectList(selectListStudents.OrderBy(x => x.Text), "Value", "Text");
        }
    }
}

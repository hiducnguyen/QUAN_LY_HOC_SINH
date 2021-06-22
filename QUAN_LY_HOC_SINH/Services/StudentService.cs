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

namespace Services
{
    public class StudentService : IStudentService
    {
        private IGenericRepository _genericRepository;
        private IStudentRepository _studentRepository;
        private IUnitOfWork _unitOfWork;

        public StudentService(
            IGenericRepository genericRepository,
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork)
        {
            _genericRepository = genericRepository;
            _studentRepository = studentRepository;
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

        private Student MapDTOtoStudent(CreateStudentDTO createStudentDTO)
        {
            return new Student
            {
                StudentId = createStudentDTO.StudentId,
                Name = createStudentDTO.Name,
                BirthDate = createStudentDTO.BirthDate,
                Email = createStudentDTO.Email,
                Address = createStudentDTO.Address,
                Gender = GenderHelper.ToGender(createStudentDTO.Gender),
                Version = (createStudentDTO.EditMode)?createStudentDTO.Version:default
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

        public void UpdateStudent(CreateStudentDTO createStudentDTO)
        {
            if (IsMissingRequiredField(createStudentDTO))
            {
                throw new MissingRequiredFieldException();
            }

            Student student = MapDTOtoStudent(createStudentDTO);
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
        private bool IsMissingRequiredField(CreateStudentDTO createStudentDTO)
        {
            return (createStudentDTO.StudentId == default) || (createStudentDTO.Name == default) ||
                (createStudentDTO.Address == default) || (createStudentDTO.BirthDate == default) ||
                (createStudentDTO.Gender == default) || (createStudentDTO.Email == default);
        }
    }
}

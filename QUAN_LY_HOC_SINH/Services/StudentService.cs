using Repositories;
using Repositories.Enums;
using Repositories.Models;
using Repositories.UnitOfWork;
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
            Student student = mapDTOtoStudent(createStudentDTO);

            using (_unitOfWork.Start())
            {
                if (_studentRepository.FindStudentByStudentId(student.StudentId) != null)
                {
                    throw new StudentIdAlreadyExistsException(student.StudentId);
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
                    throw new StudentNotExistsException(studentId);
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
                indexStudentDTOs.Add(mapStudentToIndexStudentDTO(student));
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
            CreateStudentDTO createStudentDTO = mapStudentToCreateStudentDTO(student);
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

        private Student mapDTOtoStudent(CreateStudentDTO createStudentDTO)
        {
            return new Student
            {
                StudentId = createStudentDTO.StudentId,
                Name = createStudentDTO.Name,
                BirthDate = createStudentDTO.BirthDate,
                Email = createStudentDTO.Email,
                Address = createStudentDTO.Address,
                Gender = GenderHelper.ToGender(createStudentDTO.Gender)
            };
        }
        private IndexStudentDTO mapStudentToIndexStudentDTO(Student student)
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
        private CreateStudentDTO mapStudentToCreateStudentDTO(Student student)
        {
            return new CreateStudentDTO
            {
                StudentId = student.StudentId,
                Name = student.Name,
                BirthDate = student.BirthDate,
                Email = student.Email,
                Address = student.Address,
                Gender = student.Gender.ToString()
            };
        }
    }
}

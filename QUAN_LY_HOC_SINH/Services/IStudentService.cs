using Repositories.Models;
using Services.DTO;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Services
{
    public interface IStudentService
    {
        /// <summary>
        /// Create a Student from the DTO and save it to database
        /// </summary>
        /// <param name="createStudentDTO"></param>
        /// <exception cref="Services.Exceptions.ObjectAlreadyExistsException">
        /// Throw when a student with the same StudentId has already exists 
        /// </exception>
        /// <exception cref="Services.Exceptions.MissingRequiredFieldException">
        /// Throw when one (or more) required field is (are) missed
        /// </exception>
        /// <returns></returns>
        Student CreateStudent(CreateStudentDTO createStudentDTO);
        /// <summary>
        /// Update the student is map with createStudentDTO
        /// </summary>
        /// <param name="createStudentDTO"></param>
        /// <exception cref="Services.Exceptions.ObjectNotExistsException">
        /// Throw when the student with the studentId does not exist
        /// </exception>
        /// <exception cref="Services.Exceptions.MissingRequiredFieldException">
        /// Throw when one (or more) required field is (are) missed
        /// </exception>
        /// <exception cref="Services.Exceptions.ObjectHasBeenUpdatedException"></exception>
        /// <returns></returns>
        void UpdateStudent(CreateStudentDTO createStudentDTO);
        /// <summary>
        /// Find all students and map each of them to IndexStudentDTO
        /// </summary>
        /// <returns></returns>
        IList<IndexStudentDTO> FindAllStudents();
        /// <summary>
        /// Find a student by studetnId and map it to CreateStudentDTO
        /// </summary>
        /// <exception cref="Services.Exceptions.ObjectNotExistsException"></exception>
        /// <param name="studentId"></param>
        /// <returns></returns>
        CreateStudentDTO FindStudentByStudentId(int studentId);
        /// <summary>
        /// Delete one student with the corresponding studentId
        /// </summary>
        /// <exception cref="Services.Exceptions.ObjectNotExistsException">
        /// Throw when the student with the corresponding studentId does not exist
        /// </exception>
        /// <param name="studentId"></param>
        void DeleteStudent(int studentId);
        /// <summary>
        /// Check if a studentId is available or already exist
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        bool IsStudentIdAlreadyExist(int studentId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>All student which have not had class yet</returns>
        MultiSelectList GetAllAvailableStudents();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <exception cref="Services.Exceptions.ObjectNotExistsException">
        /// Throw when the class with corresponding className not exist
        /// </exception>
        /// <returns>All student which have not had class yet and all current student of the class which have className</returns>
        MultiSelectList GetAllAvailableStudents(string className);
    }
}

using Repositories.Models;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services
{
    public interface IClassService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>All the grades of the school</returns>
        SelectList GetAllGrades();
        /// <summary>
        /// Create a Class from the DTO and save it to database
        /// </summary>
        /// <param name="createClassDTO"></param>
        /// <exception cref="Services.Exceptions.ObjectAlreadyExistsException">
        /// Throw when a class with the same name already exist
        /// </exception>
        /// <exception cref="Services.Exceptions.MissingRequiredFieldException"></exception>
        /// <exception cref="Services.Exceptions.StudentAlreadyHaveClassException"></exception>
        /// <exception cref="Services.Exceptions.OutOfMaximumNumberOfStudentsInClassException"></exception>
        /// <exception cref="Services.Exceptions.ObjectNotExistsException">
        /// Throw when a student of class not exists
        /// </exception>
        /// <returns></returns>
        Class CreateClass(CreateClassDTO createClassDTO);
        /// <summary>
        /// Check if a class name is exist or not
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsClassNameExist(string name);
        /// <summary>
        /// Find a class by name and map it to CreateClassDTO
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="Services.Exceptions.ObjectNotExistsException">
        /// Thow when the class with corresponding class name is not exist
        /// </exception>
        /// <returns></returns>
        CreateClassDTO FindClassByName(string name);
        /// <summary>
        /// Update the class is corresponding with createClassDTO
        /// </summary>
        /// <exception cref="Services.Exceptions.ObjectNotExistsException"></exception>
        /// <exception cref="Services.Exceptions.MissingRequiredFieldException"></exception>
        /// <exception cref="Services.Exceptions.ObjectHasBeenUpdatedException"></exception>
        /// <exception cref="Services.Exceptions.OutOfMaximumNumberOfStudentsInClassException"></exception>
        /// <exception cref="Services.Exceptions.StudentAlreadyHaveClassException"></exception>
        /// <param name="createClassDTO"></param>
        void UpdateClass(CreateClassDTO createClassDTO);
        /// <summary>
        /// Find all classes and map them to IndexClassDTO
        /// </summary>
        /// <returns></returns>
        IList<IndexClassDTO> FindAllClasses();
        /// <summary>
        /// Delete class corresponding with className
        /// </summary>
        /// <exception cref="Services.Exceptions.ObjectNotExistsException"></exception>
        /// <param name="className"></param>
        void DeleteClass(string className);
        /// <summary>
        /// Find all classes and convert them to select list
        /// </summary>
        /// <returns></returns>
        SelectList GetSelectListClasses();
    }
}

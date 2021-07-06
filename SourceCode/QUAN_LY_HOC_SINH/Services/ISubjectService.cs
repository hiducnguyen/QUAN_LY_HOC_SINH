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
    public interface ISubjectService
    {
        /// <summary>
        /// Find all subjects and map each of them to IndexSubjectDTO
        /// </summary>
        /// <returns></returns>
        IList<IndexSubjectDTO> FindAllSubjects();
        /// <summary>
        /// Map the DTO to Subject and save it
        /// </summary>
        /// <remarks>Create subject also create a transcript of the subject for all students</remarks>
        /// <param name="createSubjectDTO"></param>
        /// <exception cref="Services.Exceptions.MissingRequiredFieldException"></exception>
        /// <exception cref="Services.Exceptions.ObjectAlreadyExistsException"></exception>
        /// <returns></returns>
        Subject CreateSubject(CreateSubjectDTO createSubjectDTO);
        /// <summary>
        /// Check if a subject with subjectId already exist or not
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        bool IsSubjectIdAlreadyExist(int subjectId);
        /// <summary>
        /// Check if a subject with name already exist or not
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsSubjectNameAlreadyExist(string name);
        /// <summary>
        /// Delete subject correspoding with subjectId
        /// </summary>
        /// <exception cref="Services.Exceptions.ObjectNotExistsException"></exception>
        /// <param name="subjecId"></param>
        void DeleteSubject(int subjecId);
        /// <summary>
        /// Find subject by subjectId and map it to CreateSubjectDTO
        /// </summary>
        /// <param name="subjectId"></param>
        /// <exception cref="Services.Exceptions.ObjectNotExistsException"></exception>
        /// <returns></returns>
        CreateSubjectDTO FindSubjectBySubjectId(int subjectId);
        /// <summary>
        /// Update the subject corresponding with createSubjectDTO
        /// </summary>
        /// <exception cref="Services.Exceptions.ObjectNotExistsException"></exception>
        /// <exception cref="Services.Exceptions.MissingRequiredFieldException"></exception>
        /// <exception cref="Services.Exceptions.ObjectHasBeenUpdatedException"></exception>
        /// <exception cref="Services.Exceptions.ObjectAlreadyExistsException">
        /// Throw when the new name of Subject has already taken
        /// </exception>
        /// <param name="createSubjectDTO"></param>
        void UpdateSubject(CreateSubjectDTO createSubjectDTO);
        /// <summary>
        /// Find all subjects and convert them to SelectList
        /// </summary>
        /// <returns></returns>
        SelectList GetSelectListSubjects();
    }
}

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
    public class SubjectService : ISubjectService
    {
        private IUnitOfWork _unitOfWork;
        private ISubjectRepository _subjectRepository;
        private ITranscriptRepository _transcriptRepository;
        private IGenericRepository _genericRepository;

        private bool IsMissingRequiredFielad(CreateSubjectDTO createSubjectDTO)
        {
            return (createSubjectDTO.SubjectId == default) || (createSubjectDTO.Name == default);
        }

        public SubjectService(IUnitOfWork unitOfWork, ISubjectRepository subjectRepository,
            IGenericRepository genericRepository,
            ITranscriptRepository transcriptRepository)
        {
            _unitOfWork = unitOfWork;
            _subjectRepository = subjectRepository;
            _genericRepository = genericRepository;
            _transcriptRepository = transcriptRepository;
        }

        public Subject CreateSubject(CreateSubjectDTO createSubjectDTO)
        {
            if (IsMissingRequiredFielad(createSubjectDTO)) throw new MissingRequiredFieldException();
            using (_unitOfWork.Start())
            {
                if (_subjectRepository.FindSubjectBySubjectId(createSubjectDTO.SubjectId) != null)
                {
                    throw new ObjectAlreadyExistsException(Resource.Subject, Resource.Id, createSubjectDTO.SubjectId);
                }
                if (_subjectRepository.FindSubjectByName(createSubjectDTO.Name) != null)
                {
                    throw new ObjectAlreadyExistsException(Resource.Subject, Resource.Name, createSubjectDTO.Name);
                }
                Subject subject = new Subject
                {
                    Name = createSubjectDTO.Name,
                    SubjectId = createSubjectDTO.SubjectId
                };
                _genericRepository.Save(subject);
                _unitOfWork.Commit();
                return subject;
            }
        }

        public IList<IndexSubjectDTO> FindAllSubjects()
        {
            IList<Subject> subjects;
            using (_unitOfWork.Start())
            {
                subjects = _subjectRepository.FindAllSubjects();
            }
            if (subjects == null) return null;
            IList<IndexSubjectDTO> indexSubjectDTOs = new List<IndexSubjectDTO>();
            foreach (Subject subject in subjects)
            {
                IndexSubjectDTO indexSubjectDTO = new IndexSubjectDTO
                {
                    SubjectId = subject.SubjectId,
                    Name = subject.Name
                };
                indexSubjectDTOs.Add(indexSubjectDTO);
            }
            return indexSubjectDTOs;
        }

        public bool IsSubjectIdAlreadyExist(int subjectId)
        {
            using (_unitOfWork.Start())
            {
                return _subjectRepository.FindSubjectBySubjectId(subjectId) != null;
            }
        }

        public bool IsSubjectNameAlreadyExist(string name)
        {
            using (_unitOfWork.Start())
            {
                return _subjectRepository.FindSubjectByName(name) != null;
            }
        }

        public void DeleteSubject(int subjecId)
        {
            using (_unitOfWork.Start())
            {
                Subject subject = _subjectRepository.FindSubjectBySubjectId(subjecId);
                if (subject == null) throw new ObjectNotExistsException(Resource.Subject, Resource.Id, subjecId);
                IList<Transcript> transcripts = _transcriptRepository.FindAllTranscripts(subject);
                foreach (Transcript transcript in transcripts)
                {
                    _genericRepository.Delete(transcript);
                }
                _genericRepository.Delete(subject);
                _unitOfWork.Commit();
            }
        }

        public CreateSubjectDTO FindSubjectBySubjectId(int subjectId)
        {
            Subject subject;
            using (_unitOfWork.Start())
            {
                subject = _subjectRepository.FindSubjectBySubjectId(subjectId);
            }
            if (subject == null) throw new ObjectNotExistsException(Resource.Subject, Resource.Id, subjectId);
            CreateSubjectDTO createSubjectDTO = new CreateSubjectDTO
            {
                Name = subject.Name,
                SubjectId = subject.SubjectId,
                Version = subject.Version
            };
            return createSubjectDTO;
        }

        public void UpdateSubject(CreateSubjectDTO createSubjectDTO)
        {
            if (IsMissingRequiredFielad(createSubjectDTO)) throw new MissingRequiredFieldException();
            Subject subject;
            using (_unitOfWork.Start())
            {
                subject = _subjectRepository.FindSubjectBySubjectId(createSubjectDTO.SubjectId);
            }
            if (subject == null)
                throw new ObjectNotExistsException(Resource.Subject, Resource.Id, createSubjectDTO.SubjectId);
            if (subject.Version != createSubjectDTO.Version)
                throw new ObjectHasBeenUpdatedException(Resource.Subject, Resource.Id, createSubjectDTO.SubjectId);

            // Check if the new name of subject has already taken or not
            using (_unitOfWork.Start())
            {
                Subject subjectWithSameName = _subjectRepository.FindSubjectByName(createSubjectDTO.Name);
                if (subjectWithSameName != null && subjectWithSameName.SubjectId != subject.SubjectId)
                    throw new ObjectAlreadyExistsException(Resource.Subject, Resource.Name, createSubjectDTO.Name);
            }

            subject.Name = createSubjectDTO.Name;
            using (_unitOfWork.Start())
            {
                _genericRepository.Update(subject);
                _unitOfWork.Commit();
            }
        }

        public SelectList GetSelectListSubjects()
        {
            IEnumerable<SelectListItem> allSubjects;
            using (_unitOfWork.Start())
            {
                allSubjects = new List<SelectListItem>(
                    _subjectRepository.FindAllSubjects().Select(x => new SelectListItem
                    {
                        Value = x.SubjectId.ToString(),
                        Text = $"{x.SubjectId}: {x.Name}"
                    })
                );
            }

            return new SelectList(allSubjects.OrderBy(x => x.Text), "Value", "Text");
        }
    }
}

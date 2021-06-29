using Repositories.Models;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Models;
using Repositories.UnitOfWork;
using Repositories;
using Services.Exceptions;
using Resources;
using Repositories.Enums;

namespace Services
{
    public class TranscriptService : ITranscriptService
    {
        private IUnitOfWork _unitOfWork;
        private ITranscriptRepository _transcriptRepository;
        private IGenericRepository _genericRepository;
        private IClassRepository _classRepository;
        private ISubjectRepository _subjectRepository;
        private IStudentRepository _studentRepository;

        public TranscriptService(IUnitOfWork unitOfWork, 
            ITranscriptRepository transcriptRepository,
            IGenericRepository genericRepository,
            IClassRepository classRepository,
            ISubjectRepository subjectRepository,
            IStudentRepository studentRepository)
        {
            _unitOfWork = unitOfWork;
            _transcriptRepository = transcriptRepository;
            _genericRepository = genericRepository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _studentRepository = studentRepository;
        }

        public void CreateTranscript(CreateTranscriptDTO createTranscriptDTO)
        {
            using (_unitOfWork.Start())
            {
                Class @class = _classRepository.FindClassByName(createTranscriptDTO.ClassName);
                if (@class == null)
                {
                    throw new ObjectNotExistsException(Resource.Class, Resource.Name, createTranscriptDTO.ClassName);
                }
                Subject subject = _subjectRepository.FindSubjectBySubjectId(createTranscriptDTO.SubjectId);
                if (subject == null)
                {
                    throw new ObjectNotExistsException(Resource.Subject, Resource.Id, createTranscriptDTO.SubjectId);
                }
                if (_transcriptRepository.FindTranscriptsOfClass(createTranscriptDTO.ClassName, createTranscriptDTO.SubjectId,
                    (Semester)createTranscriptDTO.Semester) != null)
                {
                    throw new ObjectAlreadyExistsException(Resource.Transcript, Resource.TranscriptOfClass,
                        $"{createTranscriptDTO.SubjectId}/{createTranscriptDTO.ClassName}/{createTranscriptDTO.Semester}");
                }
                IList<Student> students = _studentRepository.FindStudentsByClassId(@class.Id);

                foreach (Student student in students)
                {
                    Transcript transcript = new Transcript
                    {
                        StudentId = student.Id,
                        Semester = (Semester)createTranscriptDTO.Semester,
                        Subject = subject
                    };
                    _genericRepository.Save(transcript);
                }
                _unitOfWork.Commit();
            }
        }

        public void DeleteTranscripts(int subjectId, string className, int semester)
        {
            using (_unitOfWork.Start())
            {
                Class @class = _classRepository.FindClassByName(className);
                if (@class == null)
                {
                    throw new ObjectNotExistsException(Resource.Class, Resource.Name, className);
                }
                Subject subject = _subjectRepository.FindSubjectBySubjectId(subjectId);
                if (subject == null)
                {
                    throw new ObjectNotExistsException(Resource.Subject, Resource.Id, subjectId);
                }
                if (_transcriptRepository.FindTranscriptsOfClass(className, subjectId,
                    (Semester)semester) == null)
                {
                    throw new ObjectNotExistsException(Resource.Transcript, Resource.TranscriptOfClass,
                        $"{subjectId}/{className}/{semester}");
                }
                IList<Student> students = _studentRepository.FindStudentsByClassId(@class.Id);
                foreach (Student student in students)
                {
                    Transcript transcript = _transcriptRepository.FindTranscript(subject, student.Id,
                        (Semester)semester);
                    _genericRepository.Delete(transcript);
                }
                _unitOfWork.Commit();
            }
        }

        public IList<IndexTranscriptDTO> FindAllTranscripts()
        {
            IList<TranscriptOfClass> transcriptOfClasses;
            using (_unitOfWork.Start())
            {
                transcriptOfClasses = _transcriptRepository.FindAllTranscriptsOfClass();
            }
            if (transcriptOfClasses == null) return null;
            IList<IndexTranscriptDTO> indexTranscriptDTOs = new List<IndexTranscriptDTO>();
            foreach (TranscriptOfClass transcriptOfClass in transcriptOfClasses)
            {
                IndexTranscriptDTO indexTranscriptDTO = new IndexTranscriptDTO
                {
                    ClassName = transcriptOfClass.ClassName,
                    SubjectName = transcriptOfClass.SubjectName,
                    SubjectId = transcriptOfClass.SubjectId,
                    Semester = (int)transcriptOfClass.Semester
                };
                indexTranscriptDTOs.Add(indexTranscriptDTO);
            }
            return indexTranscriptDTOs;
        }

        public IList<TranscriptDetailDTO> FindTranscripts(int subjectId, string className, int semester)
        {
            using (_unitOfWork.Start())
            {
                Class @class = _classRepository.FindClassByName(className);
                if (@class == null)
                {
                    throw new ObjectNotExistsException(Resource.Class, Resource.Name, className);
                }
                Subject subject = _subjectRepository.FindSubjectBySubjectId(subjectId);
                if (subject == null)
                {
                    throw new ObjectNotExistsException(Resource.Subject, Resource.Id, subjectId);
                }
                if (_transcriptRepository.FindTranscriptsOfClass(className, subjectId,
                    (Semester)semester) == null)
                {
                    throw new ObjectNotExistsException(Resource.Transcript, Resource.TranscriptOfClass,
                        $"{subjectId}/{className}/{semester}");
                }
                IList<Student> students = _studentRepository.FindStudentsByClassId(@class.Id);
                IList<TranscriptDetailDTO> transcriptDetailDTOs = new List<TranscriptDetailDTO>();
                foreach (Student student in students)
                {
                    Transcript transcript = _transcriptRepository.FindTranscript(subject, student.Id,
                        (Semester)semester);
                    TranscriptDetailDTO transcriptDetailDTO = new TranscriptDetailDTO
                    {
                        StudentName = student.Name,
                        StudentId = student.StudentId,
                        FifteenMinutesTestScore = transcript.FifteenMinuteTestScore,
                        FortyFiveMinutesTestScore = transcript.FortyFiveMinuteTestScore,
                        FinalTestScore = transcript.FinalTestScore,
                        Version = transcript.Version
                    };
                    transcriptDetailDTOs.Add(transcriptDetailDTO);
                }
                return transcriptDetailDTOs;
            }
        }

        public void UpdateTranscripts(int subjectId, string className, int semester, IList<TranscriptDetailDTO> transcriptDetailDTOs)
        {
            using (_unitOfWork.Start())
            {
                Class @class = _classRepository.FindClassByName(className);
                if (@class == null)
                {
                    throw new ObjectNotExistsException(Resource.Class, Resource.Name, className);
                }
                Subject subject = _subjectRepository.FindSubjectBySubjectId(subjectId);
                if (subject == null)
                {
                    throw new ObjectNotExistsException(Resource.Subject, Resource.Id, subjectId);
                }
                if (_transcriptRepository.FindTranscriptsOfClass(className, subjectId,
                    (Semester)semester) == null)
                {
                    throw new ObjectNotExistsException(Resource.Transcript, Resource.TranscriptOfClass,
                        $"{subjectId}/{className}/{semester}");
                }
                foreach (TranscriptDetailDTO transcriptDetail in transcriptDetailDTOs)
                {
                    Student student = _studentRepository.FindStudentByStudentId(transcriptDetail.StudentId);
                    Transcript transcript = _transcriptRepository.FindTranscript(subject, student.Id, (Semester)semester);
                    if (transcript.Version != transcriptDetail.Version)
                    {
                        throw new ObjectHasBeenUpdatedException(Resource.Transcript, Resource.TranscriptOfClass,
                            $"{subjectId}/{className}/{semester}");
                    }
                    transcript.FifteenMinuteTestScore = transcriptDetail.FifteenMinutesTestScore;
                    transcript.FortyFiveMinuteTestScore = transcriptDetail.FortyFiveMinutesTestScore;
                    transcript.FinalTestScore = transcriptDetail.FinalTestScore;
                    _genericRepository.Update(transcript);
                }
                _unitOfWork.Commit();
            }
        }
    }
}

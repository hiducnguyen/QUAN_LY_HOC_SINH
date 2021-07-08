using NUnit.Framework;
using Repositories;
using Repositories.Models;
using Repositories.UnitOfWork;
using Services;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class GenerateData
    {
        private Random rand = new Random();

        [Test]
        public void GenerateTranscripts()
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            ITranscriptRepository transcriptRepository = new TranscriptRepository(unitOfWork);
            IGenericRepository genericRepository = new GenericRepository(unitOfWork);
            using (unitOfWork.Start())
            {
                IList<Transcript> allTranscripts = transcriptRepository.FindAllTranscripts();
                foreach (Transcript transcript in allTranscripts)
                {
                    transcript.FifteenMinuteTestScore = GenerateScore();
                    transcript.FortyFiveMinuteTestScore = GenerateScore();
                    transcript.FinalTestScore = GenerateScore();
                    genericRepository.Update(transcript);
                }
                unitOfWork.Commit();
            }
        }

        private float GenerateScore()
        {
            float score = rand.Next(4, 11) + rand.Next(0, 2) * 0.5F;
            if (score > 10) score = 10;
            return score;
        }

        [Test]
        public void AutoAddStudentsToClasses()
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            IGenericRepository genericRepository = new GenericRepository(unitOfWork);
            IClassRepository classRepository = new ClassRepository(unitOfWork);
            IStudentRepository studentRepository = new StudentRepository(unitOfWork);
            IRuleRepository ruleRepository = new RuleRepository(unitOfWork);
            IClassService classService = new ClassService(unitOfWork, genericRepository,
                classRepository, studentRepository, ruleRepository);
            
            using (unitOfWork.Start())
            {
                IList<Class> allClasses = classRepository.FindAllClasses();
                foreach (Class @class in allClasses)
                {
                    while (@class.Students.Count < 40)
                    {
                        IList<Student> allAvailableStudents = studentRepository.FindAllAvailableStudents();
                        if (allAvailableStudents.Count == 0) break;
                        Student student = allAvailableStudents[rand.Next(0, allAvailableStudents.Count)];
                        student.ClassId = @class.Id;
                        @class.Students.Add(student);
                    }
                }
                unitOfWork.Commit();
            }
        }

        [Test]
        public void GenerateStudents()
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            IGenericRepository genericRepository = new GenericRepository(unitOfWork);
            IStudentRepository studentRepository = new StudentRepository(unitOfWork);
            IClassRepository classRepository = new ClassRepository(unitOfWork);
            ISubjectRepository subjectRepository = new SubjectRepository(unitOfWork);
            IStudentService studentService = new StudentService(unitOfWork, genericRepository,
                studentRepository, classRepository, subjectRepository);
            IList<CreateStudentDTO> createStudentDTOs = new List<CreateStudentDTO>();
            int currentAvailabelId = 1000;

            for (int i = 0; i < 350; i++)
            {
                createStudentDTOs.Add(GenerateCreateStudentDTO(ref currentAvailabelId));
            }


            foreach (CreateStudentDTO createStudentDTO in createStudentDTOs)
            {
                studentService.CreateStudent(createStudentDTO);
            }
        }

        private CreateStudentDTO GenerateCreateStudentDTO(ref int nextAvailableId)
        {
            CreateStudentDTO createStudentDTO = new CreateStudentDTO();
            createStudentDTO.StudentId = nextAvailableId;
            nextAvailableId++;
            createStudentDTO.Name = GenerateName();
            createStudentDTO.Address = GenerateAddress();
            createStudentDTO.Email = GenerateEmail();
            createStudentDTO.Gender = GenerateGender();
            createStudentDTO.BirthDate = GenerateDate();
            return createStudentDTO;
        }
        private string GenerateName()
        {
            string[] firstName = new string[] {"Nguyễn", "Lê", "Trần", "Đinh", "Võ", "Triệu", "Lý",
                "Tôn", "Trương", "Thái", "Hà" };
            string[] middleName = new string[] {"Thị", "Văn", "Tấn", "Hạnh", "Quốc", "Hồng", "Nhật", "Thanh",
                "Lan", "Đình", "Xuân", "Kim", "Ngọc" };
            string[] lastName = new string[] {"Đức", "Nga", "Hạnh", "Phúc", "Đạt", "Duy", "Hải", "Dũng", "Dương",
                "Ngọc", "Trãi", "Thắm", "Châu", "Ni", "Tiến", "Nhàn", "Nghi", "Quỳnh", "Như",  "Lộc",
                "Nam", "Phương", "Hiền", "Yên", "Tịnh", "Loan", "Giang", "Nguyên", "Trang", "Huệ", "Bài",
                "Thìn", "Tý", "Giáp", "Vy", "Long", "Trân", "Anh", "Hưng", "Hùng", "Ngân", "Huyền" };
            int firstNameIndex = rand.Next(0, firstName.Length - 1);
            int middleNameIndex = rand.Next(0, middleName.Length - 1);
            int lastNameIndex = rand.Next(0, lastName.Length - 1);
            return $"{firstName[firstNameIndex]} {middleName[middleNameIndex]} {lastName[lastNameIndex]}";
        }

        private string GenerateAddress()
        {
            string[] ward = new string[] {"Phường 1", "Phường 2", "Phường 3", "Phường 4",
                "Phường 5", "Phường 6", "Phường 7", "Phường 8", "Phường 9",
                "Phường 10", "Phường 11", "Phường 12"};
            string[] district = new string[] {"Quân 1", "Quận 2", "Quận 3", "Quận 4",
                "Quận 5", "Quận 6", "Quận 7", "Quận 8", "Quận 9", "Quận 10", "Quận 11",
                "Quận 12", "TP. Thủ Đức", "Huyện Hóc Môn", "Huyện Củ Chi", "Huyện Nhà Bè",
                "Huyện Cần Giờ" };
            int wardIndex = rand.Next(0, ward.Length - 1);
            int districtIndex = rand.Next(0, district.Length - 1);
            return $"{ward[wardIndex]}, {district[districtIndex]}, TP. HCM";
        }

        private string GenerateEmail()
        {
            string[] email = new string[] { "nga", "duc", "ha", "ahi", "thandong", "conna",
                "dalat", "lamdong", "daklak", "hcm", "viet", "la", "loc", "bi123", "teo11",
                "ducc112", "ti3333", "ninja2333", "iloveyou", "datohyeah"};
            int index = rand.Next(0, email.Length);
            return $"{email[index]}@gmail.com";
        }

        private string GenerateGender()
        {
            string[] gender = new string[] { "Male", "Female", "Other" };
            int index = rand.Next(0, gender.Length);
            return gender[index];
        }

        private DateTime GenerateDate()
        {
            DateTime minDate = new DateTime(2001, 1, 1);
            DateTime maxDate = new DateTime(2005, 1, 1);
            int range = ((maxDate - minDate).Days);
            return minDate.AddDays(rand.Next(range));
        }

    }
}

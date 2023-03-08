using System.Text.Json;
using System.Xml.Linq;
using UniversityManager.Entities;

namespace UniversityManager.Data
{
    public class ExamDataService
    {
        private IConfiguration _configuration;
        private string _jsonPath;

        public ExamDataService(IConfiguration configuration)
        {
            _configuration = configuration;
            _jsonPath = configuration.GetValue<string>("JsonPaths:ExamsJson");
        }

        public List<Exam> GetAll()
        {
            string json = "[]";
            if (!File.Exists(_jsonPath))
            {
                File.WriteAllText(_jsonPath, json);
            }
            else
            {
                json = File.ReadAllText(_jsonPath);
            }
            List<Exam> Exams = JsonSerializer.Deserialize<List<Exam>>(json) ?? new List<Exam>();
            return Exams;
        }

        public Exam? Get(Guid id)
        {
            List<Exam> Exams = GetAll();
            return Exams.SingleOrDefault(s => s.Id == id);
        }

        public bool Write(Exam Exam)
        {
            List<Exam> Exams = GetAll();

            //if(Exams.Any(s => s.Id == Exam.Id)) { return false; }

            Exam.Id = Guid.NewGuid();

            Exams.Add(Exam);
            SaveChanges(Exams);
            return true;
        }

        public bool Update(Exam Exam)
        {
            List<Exam> Exams = GetAll();
            Exam? old = Exams.SingleOrDefault(s => s.Id == Exam.Id);
            if (old == null)
            {
                return false;
            }
            old.Title = Exam.Title;
            old.Credits = Exam.Credits;
            SaveChanges(Exams);
            return true;
        }

        public bool Delete(Guid id)
        {
            List<Exam> Exams = GetAll();
            bool result;
            Exam? old = Exams.SingleOrDefault(s => s.Id == id);
            result = Exams.Remove(old);

            //result = Exams.RemoveAll(s => s.Id == id) == 1;
            if (!result)
                SaveChanges(Exams);
            return result;
        }

        private void SaveChanges(List<Exam> Exams)
        {
            string json = JsonSerializer.Serialize(Exams);
            File.WriteAllText(_jsonPath, json);
        }
    }
}

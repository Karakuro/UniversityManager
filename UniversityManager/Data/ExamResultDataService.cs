using System.Text.Json;
using System.Xml.Linq;
using UniversityManager.Entities;

namespace UniversityManager.Data
{
    public class ExamResultDataService
    {
        private IConfiguration _configuration;
        private string _jsonPath;

        public ExamResultDataService(IConfiguration configuration)
        {
            _configuration = configuration;
            _jsonPath = configuration.GetValue<string>("JsonPaths:ExamResultsJson");
        }

        public List<ExamResult> GetAll()
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
            List<ExamResult> ExamResults = JsonSerializer.Deserialize<List<ExamResult>>(json) ?? new List<ExamResult>();
            return ExamResults;
        }

        public ExamResult? Get(Guid examId, Guid studentId)
        {
            List<ExamResult> ExamResults = GetAll();
            return ExamResults.SingleOrDefault(s => s.ExamId == examId && s.StudentId == studentId);
        }

        public bool Write(ExamResult ExamResult)
        {
            List<ExamResult> ExamResults = GetAll();

            //if(ExamResults.Any(s => s.Id == ExamResult.Id)) { return false; }

            ExamResults.Add(ExamResult);
            SaveChanges(ExamResults);
            return true;
        }

        public bool Update(ExamResult ExamResult)
        {
            List<ExamResult> ExamResults = GetAll();
            ExamResult? old = ExamResults.SingleOrDefault(s => s.ExamId == ExamResult.ExamId && s.StudentId == ExamResult.StudentId);
            if (old == null)
            {
                return false;
            }
            old.Score = ExamResult.Score;
            SaveChanges(ExamResults);
            return true;
        }

        public bool Delete(Guid examId, Guid studentId)
        {
            List<ExamResult> ExamResults = GetAll();
            bool result;
            ExamResult? old = ExamResults.SingleOrDefault(s => s.ExamId == examId && s.StudentId == studentId);
            result = ExamResults.Remove(old);

            //result = ExamResults.RemoveAll(s => s.Id == id) == 1;
            if (!result)
                SaveChanges(ExamResults);
            return result;
        }

        private void SaveChanges(List<ExamResult> ExamResults)
        {
            string json = JsonSerializer.Serialize(ExamResults);
            File.WriteAllText(_jsonPath, json);
        }
    }
}

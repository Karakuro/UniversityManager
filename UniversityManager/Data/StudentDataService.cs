using System.Text.Json;
using UniversityManager.Entities;

namespace UniversityManager.Data
{
    public class StudentDataService
    {
        private IConfiguration _configuration;
        private string _jsonPath;

        public StudentDataService(IConfiguration configuration)
        {
            _configuration = configuration;
            _jsonPath = configuration.GetValue<string>("JsonPaths:StudentsJson");
        }

        public List<Student> GetAll()
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
            List<Student> students = JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
            return students;
        }

        public Student? Get(Guid id)
        {
            List<Student> students = GetAll();
            return students.SingleOrDefault(s => s.Id == id);
        }

        public bool Write(Student student)
        {
            List<Student> students = GetAll();

            //if(students.Any(s => s.Id == student.Id)) { return false; }

            student.Id = Guid.NewGuid();

            if(students.Any(s => s.FiscalCode == student.FiscalCode)) { return false; }

            students.Add(student);
            SaveChanges(students);
            return true;
        }

        public bool Update(Student student)
        {
            List<Student> students = GetAll();
            Student? old = students.SingleOrDefault(s => s.Id == student.Id);
            if(old == null)
            {
                return false;
            }
            old.Surname = student.Surname;
            old.Name= student.Name;
            old.BirthDate = student.BirthDate;
            SaveChanges(students);
            return true;
        }

        public bool Delete(Guid id)
        {
            List<Student> students = GetAll();
            bool result;
            Student? old = students.SingleOrDefault(s => s.Id == id);
            result = students.Remove(old);
            
            //result = students.RemoveAll(s => s.Id == id) == 1;
            if(!result)
                SaveChanges(students);
            return result;
        }

        private void SaveChanges(List<Student> students)
        {
            string json = JsonSerializer.Serialize(students);
            File.WriteAllText(_jsonPath, json);
        }
    }
}

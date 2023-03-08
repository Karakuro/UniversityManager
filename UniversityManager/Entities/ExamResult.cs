using System.ComponentModel.DataAnnotations;

namespace UniversityManager.Entities
{
    public class ExamResult
    {
        public Guid StudentId { get; set; }
        public Guid ExamId { get; set; }
        [Range(18, 31)]
        public int Score { get; set; }
    }
}

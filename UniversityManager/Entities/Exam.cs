using System.ComponentModel.DataAnnotations;

namespace UniversityManager.Entities
{
    public class Exam
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        [EnumDataType(typeof(NumCredits))]
        public NumCredits Credits { get; set; }
    }

    public enum NumCredits
    {
        Three = 3,
        Six = 6,
        Twelve = 12,
        Fifteen = 15
    }
}

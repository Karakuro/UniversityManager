using System.ComponentModel.DataAnnotations;

namespace UniversityManager.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [RegularExpression(@"^(?:[B-DF-HJ-NP-TV-Z](?:[AEIOU]{2}|[AEIOU]X)|[AEIOU]{2}X|[B-DF-HJ-NP-TV-Z]{2}[A-Z]){2}[\dLMNP-V]{2}(?:[A-EHLMPR-T](?:[04LQ][1-9MNP-V]|[1256LMRS][\dLMNP-V])|[DHPS][37PT][0L]|[ACELMRT][37PT][01LM])(?:[A-MZ][1-9MNP-V][\dLMNP-V]{2}|[A-M][0L](?:[\dLMNP-V][1-9MNP-V]|[1-9MNP-V][0L]))[A-Z]$")]
        public string FiscalCode { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

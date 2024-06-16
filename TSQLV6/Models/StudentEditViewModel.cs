namespace TSQLV6.Models
{
    public class StudentEditViewModel
    {
        public int StudentId { get; set; }
        public string StudyMode { get; set; }
        public int? SpecializationId { get; set; }
        public int? DepartmentId { get; set; }
        public int OriginalStudentId { get; set; } // To track the original student ID
    }
}

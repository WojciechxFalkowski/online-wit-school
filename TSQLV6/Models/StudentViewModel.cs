namespace TSQLV6.Models
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        public string StudyMode { get; set; }
        public int? SpecializationId { get; set; }
        public int? DepartmentId { get; set; }
        public string StudentName { get; set; }
        public string SpecializationName { get; set; }
        public string DepartmentName { get; set; }
    }


}

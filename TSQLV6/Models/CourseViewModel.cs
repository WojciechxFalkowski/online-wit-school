namespace TSQLV6.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string AcademicYear { get; set; }
        public int? LecturerId { get; set; }
        public string? LecturerEmail { get; set; }
    }
}

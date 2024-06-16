namespace TSQLV6.Models
{
    public partial class Course
    {
        public int CourseId { get; set; }
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string AcademicYear { get; set; } = null!;
        public int? LecturerId { get; set; }

        public virtual User Lecturer { get; set; } = null!;
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public virtual ICollection<Syllabus> Syllabi { get; set; } = new List<Syllabus>();
    }
}

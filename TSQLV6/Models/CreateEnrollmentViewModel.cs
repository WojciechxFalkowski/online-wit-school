namespace TSQLV6.Models
{
    public class CreateEnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        public string StudentId { get; set; } = null!;
        public int CourseId { get; set; }
        public decimal? Grade { get; set; }
        public DateOnly? CompletionDate { get; set; }
    }
}

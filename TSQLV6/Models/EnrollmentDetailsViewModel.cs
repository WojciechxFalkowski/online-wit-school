namespace TSQLV6.Models
{
    public class EnrollmentDetailsViewModel
    {
        public int EnrollmentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public decimal? Grade { get; set; }
        public string? CompletionDate { get; set; }
        public string? Points { get; set; }
    }

}

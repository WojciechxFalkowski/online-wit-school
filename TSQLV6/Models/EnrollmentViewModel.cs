using System;
using System.Collections.Generic;

namespace TSQLV6.Models;
public class EnrollmentViewModel
{
    public int EnrollmentId { get; set; }
    public string StudentName { get; set; }
    public string CourseName { get; set; }
    public decimal? Grade { get; set; }
    public string CompletionDate { get; set; }
    public string? Points { get; set; }
}

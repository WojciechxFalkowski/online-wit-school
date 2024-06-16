using System;
using System.Collections.Generic;

namespace TSQLV6.Models;

public partial class Enrollment
{
    public int EnrollmentId { get; set; }

    public string StudentId { get; set; } = null!;

    public int CourseId { get; set; }

    public decimal? Grade { get; set; }

    public DateOnly? CompletionDate { get; set; }

    public virtual Course Course { get; set; } = null!;
}

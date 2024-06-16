using System;
using System.Collections.Generic;

namespace TSQLV6.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string? StudyMode { get; set; }

    public int? SpecializationId { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Specialization? Specialization { get; set; }

    public virtual User StudentNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace TSQLV6.Models;

public partial class Syllabus
{
    public int SyllabusId { get; set; }

    public int CourseId { get; set; }

    public string? Description { get; set; }

    public string? LearningObjectives { get; set; }

    public string? AssessmentMethods { get; set; }

    public string? ReadingMaterials { get; set; }

    public virtual Course Course { get; set; } = null!;
}

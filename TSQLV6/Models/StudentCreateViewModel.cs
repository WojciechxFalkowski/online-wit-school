using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace TSQLV6.Models;
public class StudentCreateViewModel
{
    public int StudentId { get; set; }
    public string StudyMode { get; set; }
    public int? SpecializationId { get; set; }
    public int? DepartmentId { get; set; }
}


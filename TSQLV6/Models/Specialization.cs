using System;
using System.Collections.Generic;

namespace TSQLV6.Models;

public partial class Specialization
{
    public int SpecializationId { get; set; }

    public string SpecializationName { get; set; } = null!;

    public int DepartmentId { get; set; }

    public string Description { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;
}

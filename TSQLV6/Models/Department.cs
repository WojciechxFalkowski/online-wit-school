using System;
using System.Collections.Generic;

namespace TSQLV6.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();
}

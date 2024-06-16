using System;
using System.Collections.Generic;

namespace TSQLV6.Models;

public partial class Lecturer
{
    public int LecturerId { get; set; }

    public string AcademicDegree { get; set; } = null!;

    public DateOnly JoinDate { get; set; }

    public virtual User LecturerNavigation { get; set; } = null!;
}

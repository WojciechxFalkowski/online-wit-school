using System;
using System.Collections.Generic;

namespace TSQLV6.Models;

public partial class Thesis
{
    public int ThesisId { get; set; }

    public int StudentId { get; set; }

    public string Title { get; set; } = null!;

    public string ThesisType { get; set; } = null!;

    public DateTime UploadDate { get; set; }

    public string DocumentPath { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}

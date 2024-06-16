using System;
using System.Collections.Generic;

namespace TSQLV6.Models
{
    public partial class User
    {
        public int UserId { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }

        public string? UserType { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

        public virtual ICollection<Thesis> Theses { get; set; } = new List<Thesis>();
    }
}

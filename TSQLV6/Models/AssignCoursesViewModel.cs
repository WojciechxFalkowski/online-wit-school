using Microsoft.AspNetCore.Mvc.Rendering;

namespace TSQLV6.Models
{
    public class AssignCoursesViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public List<int> SelectedCourseIds { get; set; }
        public IEnumerable<SelectListItem> Courses { get; set; }
    }

}

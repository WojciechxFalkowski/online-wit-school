using System.ComponentModel.DataAnnotations;

namespace TSQLV6.Models
{
    public class SpecializationCreateViewModel
    {
        [Required(ErrorMessage = "Specialization name is required")]
        public string SpecializationName { get; set; } = null!;

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;
    }
}

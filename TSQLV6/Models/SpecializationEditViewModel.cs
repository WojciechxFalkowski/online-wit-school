using System.ComponentModel.DataAnnotations;

namespace TSQLV6.Models
{
    public class SpecializationEditViewModel
    {
        public int SpecializationId { get; set; }

        [Required(ErrorMessage = "Specialization name is required")]
        public string SpecializationName { get; set; } = null!;

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;
    }
}

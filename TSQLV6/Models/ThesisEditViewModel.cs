using System.ComponentModel.DataAnnotations;

namespace TSQLV6.Models
{
    public class ThesisEditViewModel
    {
        public int ThesisId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string ThesisType { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string DocumentPath { get; set; } = null!;
    }
}

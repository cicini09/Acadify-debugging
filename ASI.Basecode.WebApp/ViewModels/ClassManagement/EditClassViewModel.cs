using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASI.Basecode.Data.Models;

namespace Student_Performance_Tracker.ViewModels.ClassManagement
{
    public class EditClassViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }

        [Required]
        [Range(1, 3)]
        public short Semester { get; set; } = 1;

        [Required]
        [Range(1, 5)]
        public short YearLevel { get; set; } = 1;

        [Required]
        [StringLength(100)]
        public string Schedule { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Room { get; set; } = null!;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        public string? JoinCode { get; set; }

        public IEnumerable<Course>? Courses { get; set; }
        public IEnumerable<User>? Teachers { get; set; }
    }
}

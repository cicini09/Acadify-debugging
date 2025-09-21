using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public class Class
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int TeacherId { get; set; }  
        public short Semester { get; set; }
        public short YearLevel { get; set; }
        public string Schedule { get; set; } = null!;
        public string Room { get; set; } = null!;
        public string JoinCode { get; set; } = null!;
        public DateTime JoinCodeGeneratedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Course Course { get; set; } = null!;
        public virtual User Teacher { get; set; } = null!;
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
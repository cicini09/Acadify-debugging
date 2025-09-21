using System;

namespace ASI.Basecode.Data.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public virtual User Student { get; set; } = null!;
        public virtual Class Class { get; set; } = null!;
        public virtual Grade? Grade { get; set; }
    }
}
using Project.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        [ForeignKey("Mentor")]
        public string? MentorId { get; set; }
        [ForeignKey("Student")]
        public string? StudentId { get; set; }
        public ApplicationUser Mentor { get; set; }
        public ApplicationUser Student { get; set; }
    }
}

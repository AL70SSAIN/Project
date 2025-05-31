using Project.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Request
    {
        public int Id { get; set; }
        public bool Active { get; set; } = false;
        [ForeignKey("Mentor")]
        public string? MentorId { get; set; }
        [ForeignKey("Student")]
        public string? StudentId { get; set; }
        public ApplicationUser Mentor { get; set; }
        public ApplicationUser Student { get; set; }
    }
}

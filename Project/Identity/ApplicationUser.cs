using Microsoft.AspNetCore.Identity;
using Project.Models;

namespace Project.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Description { get; set; }
        public ICollection<Request> MentorRequests { get; set; } = new List<Request>();
        public ICollection<Request> StudentRequests { get; set; } = new List<Request>();
        public ICollection<Report> MentorReports { get; set; } = new List<Report>();
        public ICollection<Report> StudentReports { get; set; } = new List<Report>();

    }
}

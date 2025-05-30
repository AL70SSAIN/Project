using Project.Models;

namespace Project.ViewModels
{
    public class MentorViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Skills { get; set; } = new List<string>();
        public string AboutMe { get; set; }
        public List<Request> Requests { get; set; }

    }
}

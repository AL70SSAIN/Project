using Project.Models;

namespace Project.ViewModels
{
    public class StudentViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<Request> Requests { get; set; }
    }
}

using Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class RegisterMentorViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{11}$", ErrorMessage = "Phone number must be 11 digits.")]
        public string PhoneNumber { get; set; }
        public string? Description { get; set; }
    }
}

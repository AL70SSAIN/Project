using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class LogInUserViewModel
    {

        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

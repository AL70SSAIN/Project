using Project.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
    }
}

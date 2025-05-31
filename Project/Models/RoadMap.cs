using Project.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class RoadMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateOnly? Date { get; set; }
        public string Link { get; set; }
        [ForeignKey("Bloger")]
        public string? BlogerId { get; set; }
        public ApplicationUser Bloger { get; set; }

    }
}

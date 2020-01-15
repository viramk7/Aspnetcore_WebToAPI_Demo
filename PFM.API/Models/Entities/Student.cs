using System.ComponentModel.DataAnnotations;

namespace PFM.API.Models.Entities
{
    public class Student : Entity<int>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}

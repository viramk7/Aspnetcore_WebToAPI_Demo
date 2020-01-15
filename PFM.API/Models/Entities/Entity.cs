using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.API.Models.Entities
{
    public class Entity<TKey> : IEntity<TKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }
    }

}

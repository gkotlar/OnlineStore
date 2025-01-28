using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Category
    {
        [Required]
        public int Id { get; set; }


        [Required]
        [StringLength(50)]
        public string Name { get; set; }

    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [StringLength(450)]
        [Display(Name = "Photo URL")]
        public string? PhotoURL { get; set; }

        [StringLength(450)]
        [Display(Name = "User Manual URL")]
        public string? UserManualURL { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int ManufacturerId {  get; set; }

        public Manufacturer? Manufacturer { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}

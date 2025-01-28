using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Seller
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string Country { get; set; }


        [Display(Name = "Founding Date")]
        [DataType(DataType.Date)]
        public DateTime? FoundingDate { get; set; }

        public ICollection<Product>? Products { get; set; }

    }
}


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
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string Country { get; set; }


        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(450)]
        [Display(Name = "Photo URL")]
        [DataType(DataType.ImageUrl)]
        public string? PhotoURL { get; set; }

        [Display(Name = "Founding Date")]
        [DataType(DataType.Date)]
        public DateTime? FoundingDate { get; set; }

        public ICollection<SellerProduct>? sellerProducts { get; set; }

    }
}


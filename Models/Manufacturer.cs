using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Manufacturer
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

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(450)]
        [Display(Name = "Photo URL")]
        [DataType(DataType.ImageUrl)]
        public string? PhotoURL { get; set; }


        [Display(Name = "Founding Date")]
        [DataType (DataType.Date)]
        public DateTime? FoundingDate { get; set; }

        public ICollection<Product>? Products { get; set; }

    }
}

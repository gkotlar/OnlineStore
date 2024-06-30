using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Manufacturer
    {

        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(100)]
        [Required]
        public string Address { get; set; }

        [StringLength(100)]
        [Required]
        public string Country { get; set; }


        [Display(Name = "Founding Date")]
        [DataType (DataType.Date)]
        public DateTime? FoundingDate { get; set; }

        public ICollection<Product>? Products { get; set; }

    }
}

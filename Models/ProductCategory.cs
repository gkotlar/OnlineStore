using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace OnlineStore.Models
{
    public class ProductCategory
    {
        [Required]
        public int Id { get; set; }


        [Required]
        [Display(Name = "Category ID")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }


        [Required]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        public Product? Product { get; set; }

    }
}

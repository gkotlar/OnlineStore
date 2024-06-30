using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }



        [Display(Name = "Category ID")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }



        [Display(Name = "Product ID")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}

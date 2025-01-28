using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class SellerProduct
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public int Price { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PublishDate { get; set; }


        [Required]
        [Display(Name = "Seller ID")]
        public int SellerId { get; set; }
        public Seller? Seller { get; set; }


        [Required]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        public Product? Product { get; set; }

    }
}

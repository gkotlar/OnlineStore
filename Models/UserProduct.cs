using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class UserProduct
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required]
        [StringLength(450)]
        public string UserId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}

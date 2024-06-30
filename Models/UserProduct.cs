using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class UserProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        
        [StringLength(450)]
        public string UserId { get; set; }

        public int Quantity { get; set; }
    }
}

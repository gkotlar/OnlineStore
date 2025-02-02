using OnlineStore.Models;

namespace OnlineStore.ViewModels
{
    public class ProductReviewsViewModel
    {
        public Product Product { get; set; }
        public Review Reviews { get; set; }
        public UserProduct UserProduct { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using OnlineStore.Models;

namespace OnlineStore.ViewModels
{
    public class SellerViewModel
    {
        public Seller Seller { get; set; }

        [Display(Name = "Photo URL")]
        [DataType(DataType.ImageUrl)]
        public IFormFile? PhotoURL { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.ViewModels
{
    public class ProductCategoriesEditViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<int>? SelectedCategories { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }

        [Display(Name = "Photo URL")]
        [DataType(DataType.ImageUrl)]
        public IFormFile? PhotoURL { get; set; }

        [Display(Name = "User Manual URL")]
        [DataType(DataType.Upload)]
        public IFormFile? UserManualURL { get; set; }

    }
}

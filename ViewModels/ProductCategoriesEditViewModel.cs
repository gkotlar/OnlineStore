using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Models;
using System.Collections.Generic;

namespace OnlineStore.ViewModels
{
    public class ProductCategoriesEditViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<int>? SelectedCategories { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
    }
}

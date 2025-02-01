using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.Models;
using System.Collections.Generic;

namespace OnlineStore.ViewModels
{
    public class ProductSearchViewModel
    { 
        public IList<Product>  Products { get; set; }
        public SelectList Categories { get; set; }
        public int? ProductCategory { get; set; }
        public string? NameSearchString { get; set; }
        public string? ManufacturerSearchString { get; set; }
        public string? SellerSearchString { get; set; }
        public int? MinPriceSearchInt { get; set; }
        public int? MaxPriceSearchInt { get; set; }
        public string? SortOrder { get; set; }

        public int? MaxPrice { get; set; }
        public int? MinPrice { get; set; }
    }
}

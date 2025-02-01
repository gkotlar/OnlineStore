using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using OnlineStore.Models;

namespace OnlineStore.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Photo URL")]
        [DataType(DataType.ImageUrl)]
        public IFormFile? PhotoURL { get; set; }

        [Display(Name = "User Manual URL")]
        [DataType(DataType.Upload)]
        public IFormFile? UserManualURL { get; set; }

        [Required]
        public int ManufacturerId { get; set; }

        public Manufacturer? Manufacturer { get; set; }

        public ICollection<Review>? Reviews { get; set; }

        [NotMapped]
        public int? getReviewsCount
        {
            get
            {
                if (Reviews != null && Reviews.Any())
                {
                    return Reviews.Count();
                }
                else
                {
                    return null;
                }
            }
        }
        [NotMapped]
        public double? getReviewsAverage
        {
            get
            {
                double averageRating = 0;
                if (Reviews != null && Reviews.Any())
                {
                    int totalSum = 0;
                    int numValidReviews = 0;

                    foreach (var item in Reviews)
                    {
                        if (item.Rating.HasValue)
                        {
                            totalSum += item.Rating.Value;
                            numValidReviews++;
                        }
                    }

                    if (numValidReviews != 0)
                    {
                        averageRating = (double)totalSum / numValidReviews;
                    }

                    return Math.Round(averageRating, 2);
                }
                else
                {
                    return null;
                }

            }
        }

        [Display(Name = "Categories")]
        public ICollection<ProductCategory>? productCategories { get; set; }

        public ICollection<SellerProduct>? sellerProducts { get; set; }

        public ICollection<UserProduct>? UserProducts { get; }
    }

}

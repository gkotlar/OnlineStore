﻿using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }



        [StringLength(450)]
        public string UserId { get; set; }

        public string? Comment { get; set; }

        [Required]
        public int Rating { get; set; }

       
    }
}

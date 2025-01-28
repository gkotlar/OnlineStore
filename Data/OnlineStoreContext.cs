using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

namespace OnlineStore.Data
{
    public class OnlineStoreContext : DbContext
    {
        public OnlineStoreContext (DbContextOptions<OnlineStoreContext> options)
            : base(options)
        {
        }

        public DbSet<OnlineStore.Models.Category> Category { get; set; } = default!;
        public DbSet<OnlineStore.Models.Manufacturer> Manufacturer { get; set; } = default!;
        public DbSet<OnlineStore.Models.Product> Product { get; set; } = default!;
        public DbSet<OnlineStore.Models.ProductCategory> ProductCategory { get; set; } = default!;
        public DbSet<OnlineStore.Models.Review> Review { get; set; } = default!;
        public DbSet<OnlineStore.Models.Seller> Seller { get; set; } = default!;
        public DbSet<OnlineStore.Models.SellerProduct> SellerProduct { get; set; } = default!;
        public DbSet<OnlineStore.Models.UserProduct> UserProduct { get; set; } = default!;
    }
}


using Microsoft.EntityFrameworkCore;
using Ajerly_Platform.Models;

namespace Ajerly_Platform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Listing> Listings { get; set; }
        public DbSet<ListingImage> ListingImages { get; set; }
        public DbSet<Request> Requests { get; set; }
    }
}

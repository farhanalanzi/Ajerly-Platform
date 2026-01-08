namespace Ajerly_Platform.Models;

public class Listing
{
   
        public int Id { get; set; }

        public string Title { get; set; }

       

        public string Category { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        
        public string PriceUnit { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

      
        public virtual ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();

        // new: owner
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        
    }

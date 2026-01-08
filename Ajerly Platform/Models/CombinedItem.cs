using System;

namespace Ajerly_Platform.Models
{
    public class CombinedItem
    {
        public int Id { get; set; }
        public string? Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? Type { get; set; } = string.Empty; // "Listing" or "Request"
        public string? ImageUrl { get; set; } // Optional image URL

        // Additional optional fields used for display and filtering
        public string? Category { get; set; }
        public string? Description { get; set; }
        public string? City { get; set; }
        public decimal? Price { get; set; }
        public string? PriceUnit { get; set; }
        public string? Phone { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Ajerly_Platform.Models
{
    public class Request
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string PriceUnit { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}
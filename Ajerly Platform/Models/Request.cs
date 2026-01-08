using System.ComponentModel.DataAnnotations;

namespace Ajerly_Platform.Models
{
    public class Request
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Title { get; set; }

        [MaxLength(255)]
        public string? Category { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [MaxLength(50)]
        public string? PriceUnit { get; set; }

        [Required]
        [MaxLength(255)]
        public string? City { get; set; }

        [Required]
        [MaxLength(20)]
        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; }

        // owner
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        // Path to the image
        [MaxLength(500)]
        public string? ImagePath { get; set; }
    }
}
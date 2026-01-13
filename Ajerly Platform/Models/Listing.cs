namespace Ajerly_Platform.Models;

using System.ComponentModel.DataAnnotations;

public class Listing
{
    
        public int Id { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "العنوان لا يجب أن يتجاوز 20 أحرف/أرقام.")]
        public string? Title { get; set; }

       
        [Required]
        [MaxLength(50)]
        public string? Category { get; set; }

        [Required]
        [MaxLength(70, ErrorMessage = "تفاصيل الإعلان لا يجب أن تتجاوز 70 حرفًا/رقمًا.")]
        public string? Description { get; set; }

        [Required]
        [Range(0, 99999, ErrorMessage = "السعر يجب أن يكون رقمًا مكونًا من 1 إلى 5 خانات.")]
        [RegularExpression(@"^\d{1,5}$", ErrorMessage = "السعر يجب أن يحتوي على أرقام فقط ولا يتجاوز 5 خانات.")]
        public decimal Price { get; set; }

        
        [MaxLength(50)]
        public string? PriceUnit { get; set; }

        [Required]
        [MaxLength(12, ErrorMessage = "اسم المدينة لا يجب أن يتجاوز 12 حرفًا.")]
        public string? City { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "رقم الجوال لا يجب أن يتجاوز 10 خانات.")]
        [RegularExpression("^\\d{1,10}$", ErrorMessage = "رقم الجوال يجب أن يحتوي على أرقام فقط ولا يتجاوز 10 خانات.")]
        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

      
        public virtual ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();

        // new: owner
        [MaxLength(128)]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        
    }

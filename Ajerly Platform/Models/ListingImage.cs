namespace Ajerly_Platform.Models;

public class ListingImage
{
    public int Id { get; set; }

    public int ListingId { get; set; }

    public string ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual Listing Listing { get; set; }
    
}

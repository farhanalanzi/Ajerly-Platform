using System.Collections.Generic;

namespace Ajerly_Platform.Models
{
    public class HomeIndexViewModel
    {
        public string Category { get; set; } = "الكل";
        public List<Listing> Listings { get; set; } = new List<Listing>();
        public List<Request> Requests { get; set; } = new List<Request>();
    }
}


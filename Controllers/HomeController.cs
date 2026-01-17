public IActionResult Index()
{
    // ...existing code...

    // تأكد من أن Union يتم تطبيقه على الكيانات مباشرة
    var combinedItems = listings
        .Union(requests)
        .ToList() // تحميل البيانات من قاعدة البيانات
        .Select(item => new 
        { 
            Id = item.Id, 
            Title = item.Title, 
            Type = item is Listing ? "Listing" : "Request" 
        })
        .ToList();

    // ...existing code...
}

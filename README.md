---

# **Ajerly Platform**

Ajerly Platform is a simple and modern marketplace for renting and requesting products between individuals.  
Users can publish offers, submit requests, browse listings by category, and manage their ads easily.

---

## **Features**
- Post rental **offers** and **requests**  
- Upload  images per listing  
- Category‑based browsing and filtering  
- Clean card‑based layout for listings  
- Listing details with contact information  
- “My Ads” management page  
- Built with **ASP.NET Core (.NET 8)** and **Entity Framework Core**  
- SQLServer database for development  

---

## **Project Structure**
```
Controllers/        # MVC controllers
Models/             # Entity models
Views/              # Razor UI pages
Data/               # DbContext + migrations
wwwroot/            # Static files (CSS, JS, images)
```

---

## **Setup**
```bash
git clone <repo-url>
cd "Ajerly Platform"

dotnet restore
dotnet build

mkdir -p wwwroot/images/uploads

dotnet ef database update
dotnet run
```

Open the app at:

```
http://localhost:5000
```

---

## **Database Notes**
If you see errors like:

```
no such table: Listings
no such table: Requests
```

Reset or update the database:

```bash
rm ajerly_dev.db
dotnet ef database update
```

---

## **Contributing**
1. Fork the repo  
2. Create a feature branch  
3. Commit changes  
4. Open a pull request  

---


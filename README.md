# URL Shortener

A lightweight URL shortening service built with ASP.NET Core MVC and Entity Framework. Users can shorten URLs, manage their links, and view info about it. Administrators have full control over all URLs.

## Features

- **Shorten URLs**: Convert long URLs into short, shareable links.
- **User Authentication**: Register, login, and manage roles (Admin/User).
- **Role-Based Access**:
  - **Admins**: View, delete any URL.
  - **Users**: Create, view, and delete their own URLs.
- **Unique Code Generation**: Ensures no duplicate short URLs.
- **URL Details**: View creation date, creator, and original/shortened URLs.

## Technologies Used

- **Backend**: ASP.NET Core 6.0
- **Database**: Entity Framework Core (SQL Server/PostgreSQL)
- **Authentication**: ASP.NET Core Identity
- **Frontend**: 
  - MVC with Razor Views
- **Other**: AutoMapper, Repository Pattern, Unit of Work

## Installation

1. **Clone the Repository**:
  ```bash
   git clone https://github.com/arsenmykich/url-shortener.git
   ```
2. **Database Setup^**
```
  "ConnectionStrings": {
    "DefaultConnection": "Your-Database-Connection-String"
  }
```
3. **Run migrations**
```
add-migration migrationName
update-database
```
## Usage
# 1.**Register/Login:**
  - Navigate to /Account/Register to create an account.

  **For Admin:**
    Email: admin@example.com
    Password: Admin123!
  

# 2.**Shorten a URL:**
 - Authenticated users can enter a URL in the "Add New URL" section.
 - Shortened URLs are automatically added to the table.

# 3.**Manage URLs:**
 - **View Details:** Click "View Details" to see metadata.
 - **Delete URLs:** Admins can delete any URL; users can only delete their own.

## **Configuration**
 - **Admin Role:** Modify the IdentitySeed.cs to change default admin credentials.
 - **Short Code Length:** Adjust NumberOfCharsInShortLink in UrlService.cs.
 - **Validation: Customize** URL validation rules in the Url model.
